// Controller for managing order-related API endpoints
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Repositories.Interfaces;
using OrderManagementSystem.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService; // Service for order business logic
    private readonly IOrderRepository _orderRepo; // Repository for order data access

    public OrderController(IOrderService orderService, IOrderRepository orderRepo)
    {
        _orderService = orderService;
        _orderRepo = orderRepo;
    }

    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        // Create a new order for a customer
        var order = await _orderService.CreateOrderAsync(
            request.CustomerId,
            request.Items.Select(i => (i.ProductId, i.Quantity)).ToList(),
            request.PaymentMethod
        );
        return Ok(order);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetById(int orderId)
    {
        // Retrieve an order by its ID
        var order = await _orderRepo.GetByIdAsync(orderId);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        // Retrieve all orders (admin only)
        var orders = await _orderRepo.GetAllWithItemsAsync();
        return Ok(orders);
    }

    [HttpPut("{orderId}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int orderId, [FromBody] string status)
    {
        // Update the status of an order (admin only)
        await _orderService.UpdateOrderStatusAsync(orderId, status);
        return Ok("Order status updated.");
    }
}

public class CreateOrderRequest
{
    public int CustomerId { get; set; } // ID of the customer placing the order
    public List<OrderItemRequest> Items { get; set; } = new(); // List of items in the order
    public string PaymentMethod { get; set; } = string.Empty; // Payment method for the order
}

public class OrderItemRequest
{
    public int ProductId { get; set; } // ID of the product being ordered
    public int Quantity { get; set; } // Quantity of the product
}
