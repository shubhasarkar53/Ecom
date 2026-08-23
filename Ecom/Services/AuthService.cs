using Ecom.Data;
using Ecom.DTOs.Auth;
using Ecom.Models;
using Ecom.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Ecom.Services
{
    public class AuthService:IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = new PasswordHasher <User> ();
            _configuration = configuration;
        }

        //register

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // Check if email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Role = "Customer",
                CreatedOn = DateTime.UtcNow
            };

            // Hash password
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }

        //login
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(user,user.PasswordHash,request.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }


        private string GenerateJwtToken(User user)
        {
            //get the key from appsettings.json/secret
            var jwtKey = _configuration["Jwt:Key"];
            //check if empty
            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("Jwt Key is not present.");
            }
            //generate claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.Role.ToString()),
                new Claim(ClaimTypes.Name,user.Email)
            };
            //generate key - utf8,key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            //generate credentials - key,algo
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //generate token
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddHours(1), signingCredentials: credential);
            //retunr token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

