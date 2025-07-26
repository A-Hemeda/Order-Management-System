namespace OrderManagementSystem.Models
{
    /// Represents a customer in the order management system.
    /// This entity contains customer information and maintains a collection of their orders.
    public class Customer
    {
        public int CustomerId { get; set; } // Unique identifier for the customer
        public string Name { get; set; } = string.Empty; // Customer's full name
        public string Email { get; set; } = string.Empty; // Customer's email address

        public ICollection<Order> Orders { get; set; } = new List<Order>(); // Orders placed by the customer
    }
}
