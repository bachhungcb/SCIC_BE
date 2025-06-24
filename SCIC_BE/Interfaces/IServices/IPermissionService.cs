using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.DTO.PermissionDataRequestDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices
{
    public interface IPermissionService
    {
        Task<List<PermissionDTO>> GetListPermissionsAsync();
        Task<PermissionDTO> GetPermissionByPermissonIdAsync(Guid id);
        Task<PermissionModel> CreatePermission(PermissionDataRequestDTO request); //<List<PermissionModel>>
        Task<PermissionModel> UpdatePermission(Guid PermissionId, PermissionDataRequestDTO request);
        Task DeletePermission(Guid PermisionId);
    }
}
