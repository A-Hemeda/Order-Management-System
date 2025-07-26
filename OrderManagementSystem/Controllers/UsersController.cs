// Controller for managing user authentication and registration endpoints
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService; // Service for user authentication and registration

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // Register a new user
        await _userService.RegisterAsync(request.Username, request.Password, request.Role);
        return Ok("Registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Authenticate user and return JWT token
        var token = await _userService.LoginAsync(request.Username, request.Password);
        return Ok(new { Token = token });
    }
}

public class RegisterRequest
{
    public string Username { get; set; } // Username for registration
    public string Password { get; set; } // Password for registration
    public string Role { get; set; } = "Customer"; // User role (default: Customer)
}

public class LoginRequest
{
    public string Username { get; set; } // Username for login
    public string Password { get; set; } // Password for login
}
