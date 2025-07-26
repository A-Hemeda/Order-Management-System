using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Interfaces;
using OrderManagementSystem.Services.Interfaces;

namespace OrderManagementSystem.Services.Classes
{
    /// <summary>
    /// Service for managing order operations
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo; // Repository for order data access
        private readonly IProductRepository _productRepo; // Repository for product data access
        private readonly ICustomerRepository _customerRepo; // Repository for customer data access
        private readonly IRepository<Invoice> _invoiceRepo; // Repository for invoice data access
        private readonly IEmailService _emailService; // Service for sending emails
        private readonly ILogger<OrderService> _logger; // Logger for this service

        public OrderService(
            IOrderRepository orderRepo,
            IProductRepository productRepo,
            ICustomerRepository customerRepo,
            IRepository<Invoice> invoiceRepo,
            IEmailService emailService,
            ILogger<OrderService> logger)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _customerRepo = customerRepo;
            _invoiceRepo = invoiceRepo;
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new order with the specified items
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <param name="items">List of product IDs and quantities</param>
        /// <param name="paymentMethod">Payment method</param>
        /// <returns>The created order</returns>
        public async Task<Order> CreateOrderAsync(int customerId, List<(int productId, int quantity)> items, string paymentMethod)
        {
            // Create a new order for a customer
            _logger.LogInformation("Creating order for customer {CustomerId} with {ItemCount} items", customerId, items.Count);
            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer {CustomerId} not found for order creation", customerId);
                throw new Exception("Customer not found");
            }

            var orderItems = new List<OrderItem>();
            decimal total = 0;

            foreach (var (productId, quantity) in items)
            {
                var product = await _productRepo.GetByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning("Product {ProductId} not found for order creation", productId);
                    throw new Exception($"Product {productId} not found");
                }

                if (product.Stock < quantity)
                {
                    _logger.LogWarning("Insufficient stock for product {ProductName} (requested: {Quantity}, available: {Stock})", 
                        product.Name, quantity, product.Stock);
                    throw new Exception($"Insufficient stock for product {product.Name}");
                }

                var itemTotal = product.Price * quantity;
                total += itemTotal;

                // Update product stock
                product.Stock -= quantity;
                await _productRepo.UpdateAsync(product);
                _logger.LogInformation("Updated stock for product {ProductName}: {NewStock}", product.Name, product.Stock);

                orderItems.Add(new OrderItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    Discount = 0
                });
            }

            // Calculate discount based on total
            decimal discount = 0;
            if (total > 200) discount = 0.10m;
            else if (total > 100) discount = 0.05m;

            foreach (var item in orderItems)
            {
                item.Discount = item.UnitPrice * discount;
            }

            var order = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                PaymentMethod = paymentMethod,
                Status = "Pending",
                OrderItems = orderItems,
                TotalAmount = total - (total * discount)
            };

            await _orderRepo.AddAsync(order);
            _logger.LogInformation("Created order {OrderId} for customer {CustomerId} with total amount {TotalAmount}", 
                order.OrderId, customerId, order.TotalAmount);

            // Create invoice for the order
            var invoice = new Invoice
            {
                OrderId = order.OrderId,
                InvoiceDate = DateTime.UtcNow,
                TotalAmount = order.TotalAmount
            };

            await _invoiceRepo.AddAsync(invoice);
            _logger.LogInformation("Created invoice {InvoiceId} for order {OrderId}", invoice.InvoiceId, order.OrderId);

            return order;
        }

        /// <summary>
        /// Updates the status of an existing order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="newStatus">The new status</param>
        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            // Update the status of an order and notify the customer
            _logger.LogInformation("Updating order {OrderId} status to {NewStatus}", orderId, newStatus);
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning("Order {OrderId} not found for status update", orderId);
                throw new Exception("Order not found");
            }

            var customer = await _customerRepo.GetByIdAsync(order.CustomerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer {CustomerId} not found for order {OrderId} status update", order.CustomerId, orderId);
                throw new Exception("Customer not found");
            }

            var oldStatus = order.Status;
            order.Status = newStatus;
            await _orderRepo.UpdateAsync(order);
            _logger.LogInformation("Updated order {OrderId} status from {OldStatus} to {NewStatus}", 
                orderId, oldStatus, newStatus);

            // Send email notification to the customer
            string subject = "Order Status Updated";
            string body = $"Hello {customer.Name},\n\nYour order #{order.OrderId} status has been updated to: {newStatus}.";
            try
            {
                await _emailService.SendOrderStatusEmailAsync(customer.Email, subject, body);
                _logger.LogInformation("Sent status update email to customer {CustomerEmail} for order {OrderId}", 
                    customer.Email, orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send status update email to customer {CustomerEmail} for order {OrderId}", 
                    customer.Email, orderId);
                // Don't throw here - order status was updated successfully
            }
        }
    }
}
