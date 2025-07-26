using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.DTOs
{
    /// <summary>
    /// Response DTO for Product entity
    /// </summary>
    public class ProductResponse
    {
        public int ProductId { get; set; } // Unique identifier for the product
        public string Name { get; set; } = string.Empty; // Product name
        public decimal Price { get; set; } // Product price
        public int Stock { get; set; } // Available stock quantity
    }

    /// <summary>
    /// Request DTO for creating a new Product
    /// </summary>
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty; // Name for the new product

        [Required(ErrorMessage = "Product price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; } // Price for the new product

        [Required(ErrorMessage = "Product stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be 0 or greater")]
        public int Stock { get; set; } // Initial stock quantity
    }

    /// <summary>
    /// Request DTO for updating an existing Product
    /// </summary>
    public class UpdateProductRequest
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty; // Updated product name

        [Required(ErrorMessage = "Product price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; } // Updated product price

        [Required(ErrorMessage = "Product stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be 0 or greater")]
        public int Stock { get; set; } // Updated stock quantity
    }
} 