namespace OrderManagementSystem.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; } // Unique identifier for the order item
        public int OrderId { get; set; } // Foreign key to the order
        public int ProductId { get; set; } // Foreign key to the product

        public int Quantity { get; set; } // Quantity of the product ordered
        public decimal UnitPrice { get; set; } // Price per unit at the time of order
        public decimal Discount { get; set; } // Discount applied to this item

        // Navigation
        public Order Order { get; set; } // Navigation property to the order
        public Product Product { get; set; } // Navigation property to the product
    }
}
