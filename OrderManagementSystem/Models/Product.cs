namespace OrderManagementSystem.Models
{
    public class Product
    {
        public int ProductId { get; set; } // Unique identifier for the product
        public string Name { get; set; } = string.Empty; // Product name
        public decimal Price { get; set; } // Product price
        public int Stock { get; set; } // Available stock quantity
    }
}
