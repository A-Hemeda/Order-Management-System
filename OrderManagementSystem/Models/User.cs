namespace OrderManagementSystem.Models
{
    public class User
    {
        public int UserId { get; set; } // Unique identifier for the user
        public string Username { get; set; } = string.Empty; // Username for login
        public string PasswordHash { get; set; } = string.Empty; // Hashed password
        public string Role { get; set; } = "Customer"; // User role (e.g., Customer, Admin)
    }
}
