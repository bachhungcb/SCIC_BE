using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices
{
    public interface IUserService
    {
        Task<UserDTO> GetUserAsync(Guid id);
        Task<List<UserDTO>> GetListUserAsync();
        Task<List<UserDTO>> GetListUserWithDefaultRoleAsync();
        Task CreateUserAsync(CreateUserDTO dto);
        Task UpdateUserAsync(Guid id, UpdateUserDTO dto);
        Task DeleteUserAsync(Guid id);
    }
}
