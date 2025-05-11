using SCIC_BE.DTO.PermissionDataRequestDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices
{
    public interface IPermissionService
    {
        Task<List<PermissionModel>> GetListPermissionsAsync();
        Task<PermissionModel> GetPermissionByPermissonIdAsync(Guid id);
        Task CreatePermission(PermissionDataRequestDTO request);
    }
}
