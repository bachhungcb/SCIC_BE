using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task AddUserAsync(UserModel user);
        Task<UserDTO> GetUserByIdAsync(Guid id);
        Task<UserModel> GetUserEntityByIdAsync(Guid id);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserModel> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(UserModel user);
        Task DeleteUserAsync(Guid id);
    }
}
