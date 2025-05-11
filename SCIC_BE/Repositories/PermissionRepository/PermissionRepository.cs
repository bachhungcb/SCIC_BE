using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.DTO.PermissionDataRequestDTOs;
using SCIC_BE.Models;
using System.Runtime.InteropServices;

namespace SCIC_BE.Repositories.PermissionRepository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ScicDbContext _context;
        public PermissionRepository(ScicDbContext context)
        {
            _context = context;
        }

        public async Task<List<PermissionModel>> GetAllPermissionsAsync()
        {
            var permissions = await _context.Permissions.ToListAsync();

            return permissions;
        }
        public async Task<PermissionModel> GetPermissionsByIdAsync(Guid id)
        {
            var permission = await _context.Permissions.FirstOrDefaultAsync(x => x.Id == id);

            return permission; 
        }

        public async Task AddPermissionAsync(PermissionModel requestInfo)
        {
            _context.Permissions.Add(requestInfo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePermissionAsync(PermissionModel requestInfo)
        {
            var existingPermission = await GetPermissionsByIdAsync(requestInfo.Id);

            if (existingPermission == null)
            {
                throw new KeyNotFoundException("Permission not found to update");
            }

            _context.Permissions.Update(requestInfo);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePermissionAsync(Guid id)
        {
            var existingPermission = await GetPermissionsByIdAsync(id);

            if (existingPermission == null)
            {
                throw new KeyNotFoundException("Permission not found to Delete");
            }

            _context.Permissions.Remove(existingPermission);
            await _context.SaveChangesAsync();
        }
    }
}
