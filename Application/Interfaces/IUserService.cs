using Application.Models;
using Common.Response;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<QLResponseUserModel> AddUsersServiceAsync(UserModel user);
        Task<ServiceResponse<QLResponseUserModel>> GetUserLoginServiceAsync(string email, string password);
            
    }
}
