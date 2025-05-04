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

        public async Task<ServiceResponse<QLResponseUserModel>> GetUserLoginServiceAsync(string email, string password)
        {
            ValidateCredentials(email, password);
            try
            {
                var existingUser = await _userRepository.GetUserLoginAsync(email, password);
                if (existingUser == null)
                {

                    return new ServiceResponse<QLResponseUserModel>
                    {
                        Success = false,
                        Message = "User Not Exist, Please Sign In.",
                        Data = null
                    };
                }
            
               return new ServiceResponse<QLResponseUserModel>
               {
                   Success = true,
                   Message = "User Detials",
                   Data = new QLResponseUserModel
                   {
                       Email = existingUser.email,
                       FirstName = existingUser.first_name,
                       LastName = existingUser.last_name,
                       CreatedAt = existingUser.created_at,
                       UpdatedAt = existingUser.updated_at
                   }
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
