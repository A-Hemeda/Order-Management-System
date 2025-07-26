using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.DTOs
{
    /// <summary>
    /// Response DTO for User entity
    /// </summary>
    public class UserResponse
    {
        public int UserId { get; set; } // Unique identifier for the user
        public string Username { get; set; } = string.Empty; // Username of the user
        public string Role { get; set; } = string.Empty; // Role of the user (Admin or Customer)
    }

    /// <summary>
    /// Request DTO for user registration
    /// </summary>
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores")]
        public string Username { get; set; } = string.Empty; // Username for registration

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty; // Password for registration

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("^(Admin|Customer)$", ErrorMessage = "Role must be either 'Admin' or 'Customer'")]
        public string Role { get; set; } = "Customer"; // Role for the new user
    }

    /// <summary>
    /// Request DTO for user login
    /// </summary>
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty; // Username for login

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty; // Password for login
    }
} 