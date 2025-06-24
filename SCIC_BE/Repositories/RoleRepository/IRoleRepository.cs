using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.DTO.RoleDTOs;
using SCIC_BE.Models;


namespace SCIC_BE.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        Task<List<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> GetRoleByNameAsync(string name);
        Task AddRoleAsync(RoleDTO roleDTO);
        Task<string> GetRoleNameByRoleIdAsync(int roleId);
    }
}
