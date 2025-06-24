using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task AddUserAsync(UserModel user);
        Task<UserModel> GetUserByIdAsync(Guid id);
        Task<List<UserModel>> GetAllUsersAsync();
        Task<UserModel> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(UserModel user);
        Task DeleteUserAsync(Guid id);
    }
}
