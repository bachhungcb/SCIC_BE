using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.RoleRepository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ScicDbContext _context;

        public UserRoleRepository(ScicDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserRoleModel userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetRolesByUserIdAsync(Guid userId)
        {
            return await _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .Select(ur => ur.Role.Name)
                    .ToListAsync();
        }

        public async Task RemoveAsync(UserRoleModel userRole)
        {
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
        }

    }
}
