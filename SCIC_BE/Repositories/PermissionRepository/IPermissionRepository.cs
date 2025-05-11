using SCIC_BE.DTO.PermissionDataRequestDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.PermissionRepository
{
    public interface IPermissionRepository
    {
        Task<List<PermissionModel>> GetAllPermissionsAsync();
        Task<PermissionModel> GetPermissionsByIdAsync(Guid id);
        Task AddPermissionAsync(PermissionModel requestInfo);
        Task UpdatePermissionAsync(PermissionModel requestInfo);
        Task DeletePermissionAsync(Guid id);
    }
}
