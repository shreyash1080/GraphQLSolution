using Application.Interfaces;
using Application.Models;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Common.Response;
using Avro.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration configuration;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _logger = logger;
            this.configuration = configuration;
        }

        // In UserService.cs
        public async Task<string> GetUserLoginServiceAsync(string email, string password)
        {
            ValidateCredentials(email, password);
            try
            {
                var existingUser = await _userRepository.GetUserLoginAsync(email, password);
                if (existingUser == null)
                {
                    return "Invalid credentials";
                }

                var claims = new[]
                {
                   new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"] ?? string.Empty),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                   new Claim("email", existingUser.email.ToString()),
                   new Claim("user_id", existingUser.user_id.ToString()),
               };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                Console.WriteLine(key);

                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: signIn
                );

                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);


                return tokenValue;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Authentication error");
                return "Authentication failed";
            }
        }
        private static void ValidateCredentials(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required", nameof(email));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required", nameof(password));

            if (!IsValidEmailFormat(email))
                throw new ArgumentException("Invalid email format", nameof(email));
        }

        private static bool IsValidEmailFormat(string email)
        {
            try
            {
                return new MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }


        public async Task<QLResponseUserModel> AddUsersServiceAsync(UserModel user)
        {
            // Map UserModel to Users entity
            var userEntity = new Users
            {
                email = user.Email,
                password_hash = HashPassword(user.Password), // Ensure password is hashed
                first_name = user.FirstName,
                last_name = user.LastName,
            };

            // Call repository to add user
            var addedUser = await _userRepository.AddUsersAsync(userEntity);

            // Map Users entity back to UserModel (password excluded)
            return new QLResponseUserModel
            {
                Email = addedUser.email,
                FirstName = addedUser.first_name,
                LastName = addedUser.last_name,
                CreatedAt = addedUser.created_at,
                UpdatedAt = addedUser.updated_at            };
        }

        // Example password hashing function
        private string HashPassword(string password)
        {
            // Implement a secure hashing mechanism (e.g., BCrypt)
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


    }
}
