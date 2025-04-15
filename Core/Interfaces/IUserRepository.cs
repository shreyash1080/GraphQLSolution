using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<Users?> GetUserLoginAsync(string email, string password);
        Task<Users> AddUsersAsync(Users user);

    }
}
