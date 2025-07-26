using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Interfaces;
using OrderManagementSystem.Services.Interfaces;
using OrderManagementSystem;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo; // Repository for user data access
    private readonly JwtSettings _jwtSettings; // JWT settings for token generation

    public UserService(IUserRepository userRepo, IOptions<JwtSettings> jwtOptions)
    {
        _userRepo = userRepo;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task RegisterAsync(string username, string password, string role)
    {
        // Register a new user with hashed password
        var existing = await _userRepo.GetByUsernameAsync(username);
        if (existing != null) throw new Exception("Username already exists");

        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password), // Hash the password
            Role = role
        };

        await _userRepo.AddAsync(user);
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        // Authenticate user and generate JWT token
        var user = await _userRepo.GetByUsernameAsync(username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Invalid credentials");

        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key); // JWT signing key

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
