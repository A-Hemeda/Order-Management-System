using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.DTOs
{
    /// <summary>
    /// Response DTO for Customer entity
    /// </summary>
    public class CustomerResponse
    {
        public int CustomerId { get; set; } // Unique identifier for the customer
        public string Name { get; set; } = string.Empty; // Customer's full name
        public string Email { get; set; } = string.Empty; // Customer's email address
    }

    /// <summary>
    /// Request DTO for creating a new Customer
    /// </summary>
    public class CreateCustomerRequest
    {
        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Customer name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty; // Name for the new customer

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        public string Email { get; set; } = string.Empty; // Email for the new customer
    }

    /// <summary>
    /// Request DTO for updating an existing Customer
    /// </summary>
    public class UpdateCustomerRequest
    {
        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Customer name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty; // Updated customer name

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        public string Email { get; set; } = string.Empty; // Updated customer email
    }
} 