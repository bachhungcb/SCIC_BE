using SCIC_BE.DTO.PermissionDataRequestDTOs;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.PermissionRepository;

namespace SCIC_BE.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<List<PermissionModel>> GetListPermissionsAsync()
        {
            var permissions = await _permissionRepository.GetAllPermissionsAsync();
            return permissions;
        }

        public async Task<PermissionModel> GetPermissionByPermissonIdAsync(Guid id)
        {
            var permission = await _permissionRepository.GetPermissionsByIdAsync(id);

            return permission;
        }

        public async Task CreatePermission(PermissionDataRequestDTO request)
        {

        }
    }
}
