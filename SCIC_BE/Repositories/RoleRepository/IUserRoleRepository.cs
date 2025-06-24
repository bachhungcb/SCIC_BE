using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.RoleRepository
{
    public interface IUserRoleRepository
    {
        Task<List<UserRoleDTO>> GetListUserRoleAsync();
        Task AddAsync(UserRoleModel userRole);
        Task<List<string>> GetRolesByUserIdAsync(Guid userId);
        Task RemoveAsync(UserRoleModel userRole);
        Task RemoveDefaultRoleForUserAsync(Guid userId);
        
    }
}
