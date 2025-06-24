using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Models;
using SCIC_BE.Repositories.UserRepository;

namespace SCIC_BE.Repositories.RoleRepository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ScicDbContext _context;
        private readonly IUserRepository _userRepository;
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

        public async Task<List<UserRoleDTO>> GetListUserRoleAsync()
        {
            var userRoles = await _context.UserRoles.ToListAsync();
            
            if (userRoles == null)
            {
                return null;
            }
            
            var userRoleDto = userRoles.Select(r => new UserRoleDTO
            {
                RoleId = r.RoleId,
            }).ToList();
            return userRoleDto;
        }

        public async Task RemoveDefaultRoleForUserAsync(Guid userId)
        {
            var userRoleToDelete = _context.UserRoles.Where(ur => ur.UserId == userId && ur.RoleId == 4);
            
            _context.UserRoles.RemoveRange(userRoleToDelete);
            
            await _context.SaveChangesAsync();
        }
    }
}
