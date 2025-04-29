using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SCIC_BE.Data;
using SCIC_BE.DTO.RoleDTO;

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
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);

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
            var role = new RoleDTO { Name = roleDTO.Name };
            await _context.AddAsync(role);
            await _context.SaveChangesAsync();
        }
    }
}
