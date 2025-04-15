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

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserModel> GetUserLoginServiceAsync(string email,
        string password)
        {
            ValidateCredentials(email, password);
            try
            {
                var existingUser = await _userRepository.GetUserLoginAsync(email, password);
                return new UserModel
                {
                    Email = existingUser.email,
                    FirstName = existingUser?.first_name,
                    LastName = existingUser?.last_name,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Please Enter Correct Credentials");
                throw new AuthenticationException("Authentication failed", ex);
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


        public async Task<UserModel> AddUsersServiceAsync(UserModel user)
        {
            // Map UserModel to Users entity
            var userEntity = new Users
            {
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow,
                email = user.Email,
                password_hash = user.Password, // Hash password in production
                first_name = user.FirstName,
                last_name = user.LastName,
            };

            // Call repository to add user
            var addedUser = await _userRepository.AddUsersAsync(userEntity);

            // Map Users entity back to UserModel
            return new UserModel
            {
                Email = addedUser.email,
                Password = null, // Do not return password in response
                FirstName = addedUser.first_name,
                LastName = addedUser.last_name,
                CreatedAt = addedUser.created_at,
                UpdatedAt = addedUser.updated_at,
            };
        }

    }
}
