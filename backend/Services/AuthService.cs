using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Data;
using backend.DTOs.Auth;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == request.Email);
        if (exists)
            throw new InvalidOperationException("Email already exists");

        var publicRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "public")
            ?? throw new InvalidOperationException("Role not found");

        var user = new User
        {
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FullName = request.FullName,
            RoleId = publicRole.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Token = GenerateToken(user, publicRole.Name),
            Email = user.Email,
            FullName = user.FullName ?? string.Empty,
            Role = publicRole.Name
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email)
            ?? throw new UnauthorizedAccessException("Invalid email or password");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new UnauthorizedAccessException("Invalid email or password");

        return new AuthResponse
        {
            Token = GenerateToken(user, user.Role.Name),
            Email = user.Email,
            FullName = user.FullName ?? string.Empty,
            Role = user.Role.Name
        };
    }

    private string GenerateToken(User user, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"]!));

        var claims = new[] 
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}