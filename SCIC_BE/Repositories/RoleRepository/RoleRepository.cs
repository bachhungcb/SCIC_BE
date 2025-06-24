using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SCIC_BE.Data;
using SCIC_BE.DTO.RoleDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ScicDbContext _context;

        public RoleRepository(ScicDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleDTO>> GetAllRolesAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            if (roles == null)
            {
                return null;
            }
            var roleDTOs = roles.Select(r => new RoleDTO
            {
                Id = r.Id,
                Name = r.Name,
            }).ToList();

            return roleDTOs;
        }

        public async Task<RoleDTO> GetRoleByNameAsync(string name)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower());

            if (role == null)
            {
                return null;
            }

            var roleDTO = new RoleDTO
            {
                Id = role.Id,
                Name = role.Name,
            };

            return roleDTO;
        }

        public async Task AddRoleAsync(RoleDTO roleDTO)
        {
            // Chuyển từ RoleDTO sang RoleModel
            var roleModel = new RoleModel
            {
                Id = roleDTO.Id,  // Gán các thuộc tính từ RoleDTO sang RoleModel
                Name = roleDTO.Name
            };

            // Thêm RoleModel vào DbContext, không phải RoleDTO
            await _context.Roles.AddAsync(roleModel);
            await _context.SaveChangesAsync();
        }

        public Task<string> GetRoleNameByRoleIdAsync(int roleId)
        {
            var role = _context.Roles.Where(r => r.Id == roleId);
            return Task.FromResult(role.Select(r => r.Name).First());
        }

    }
}
