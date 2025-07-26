namespace OrderManagementSystem.Models
{
    public class Order
    {
        public int OrderId { get; set; } // Unique identifier for the order
        public int CustomerId { get; set; } // Foreign key to the customer
        public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Date and time when the order was placed
        public decimal TotalAmount { get; set; } // Total amount for the order
        public string PaymentMethod { get; set; } = string.Empty; // Payment method used
        public string Status { get; set; } = "Pending"; // Current status of the order

        // Navigation
        public Customer Customer { get; set; } // Navigation property to the customer
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Items in the order
        public Invoice Invoice { get; set; } // Associated invoice
    }
}
