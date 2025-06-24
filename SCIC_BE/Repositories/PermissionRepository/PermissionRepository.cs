using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.DTO.PermissionDataRequestDTOs;
using SCIC_BE.Models;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SCIC_BE.DTO.UserDTOs;

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
            var permissions = await _context.Permissions
                                            .Include(p => p.PermissionUsers)
                                            .ThenInclude(pu => pu.User).ToListAsync();
            

            return permissions;
        }
        
        public async Task<List<PermissionDTO>> GetAllPermissionsDtoAsync()
        {
            var permissions = await _context.Permissions
                .Include(p => p.PermissionUsers)
                .ThenInclude(pu => pu.User)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();

            var permissionsList = new List<PermissionDTO>();
            foreach (var permission in permissions)
            {
                if (permission == null) return null;

                var permissionDto = new PermissionDTO
                {
                    Id = permission.Id,
                    TimeStart = permission.TimeStart,
                    TimeEnd = permission.TimeEnd,
                    CreatedAt = permission.CreatedAt,
                    DeviceIds = permission.DeviceId,
                    Users = permission.PermissionUsers.Select(pu => new UserDTO()
                    {
                        Id = pu.User.Id,
                        FullName = pu.User.FullName,
                        UserName = pu.User.UserName,
                        Email = pu.User.Email,
                        IdNumber = pu.User.IdNumber,
                        FaceImage = pu.User.FaceImage,
                        UserRoles = pu.User.UserRoles
                            .Select(ur => ur.Role.Name)
                            .ToList()
                    }).ToList()
                };
                
                permissionsList.Add(permissionDto);
            }

            return permissionsList;
        }
        
        public async Task<PermissionModel> GetPermissionsByIdAsync(Guid id)
        {
            var permission = await _context.Permissions
                .Include(p => p.PermissionUsers)
                .ThenInclude(pu => pu.User)
                .FirstOrDefaultAsync(x => x.Id == id);

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
        
        public async Task<PermissionDTO> GetPermissionDToByIdAsync(Guid id)
        {
            var permission = await _context.Permissions
                .Include(p => p.PermissionUsers)
                .ThenInclude(pu => pu.User)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (permission == null) return null;

            return new PermissionDTO
            {
                Id = permission.Id,
                TimeStart = permission.TimeStart,
                TimeEnd = permission.TimeEnd,
                CreatedAt = permission.CreatedAt,
                DeviceIds = permission.DeviceId,
                Users = permission.PermissionUsers.Select(pu => new UserDTO()
                {
                    Id = pu.User.Id,
                    FullName = pu.User.FullName,
                    UserName = pu.User.UserName,
                    Email = pu.User.Email,
                    IdNumber = pu.User.IdNumber,
                    FaceImage = pu.User.FaceImage,
                    UserRoles = pu.User.UserRoles
                        .Select(ur => ur.Role.Name)
                        .ToList()
                }).ToList()
            };
        }

    }
}
