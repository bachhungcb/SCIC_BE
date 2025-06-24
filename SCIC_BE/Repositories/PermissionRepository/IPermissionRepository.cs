using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.DTO.PermissionDataRequestDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.PermissionRepository
{
    public interface IPermissionRepository
    {
        Task<List<PermissionModel>> GetAllPermissionsAsync();
        Task<PermissionModel> GetPermissionsByIdAsync(Guid id);
        Task<List<PermissionDTO>> GetAllPermissionsDtoAsync();
        Task<PermissionDTO> GetPermissionDToByIdAsync(Guid id);
        Task AddPermissionAsync(PermissionModel requestInfo);
        Task UpdatePermissionAsync(PermissionModel requestInfo);
        Task DeletePermissionAsync(Guid id);
    }
}
