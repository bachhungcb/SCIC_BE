using SCIC_BE.DTO.PermissionDataRequestDTOs;
using SCIC_BE.DTO.RcpDTOs;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.PermissionRepository;
using SCIC_BE.Repositories.UserRepository;
using System.Runtime.InteropServices;

namespace SCIC_BE.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly RcpService _rcpService;
        public PermissionService(IPermissionRepository permissionRepository,
                                 IUserRepository userRepository,
                                 RcpService rcpService)
        {
            _permissionRepository = permissionRepository;
            _userRepository = userRepository;
            _rcpService = rcpService;
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

        public async Task<List<PermissionModel>> CreatePermission(PermissionDataRequestDTO request)
        {
            var createdPermissions = new List<PermissionModel>();

            foreach (var userId in request.UserIds)
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                foreach (var deviceId in request.DeviceIds)
                {
                    var permission = new PermissionModel
                    {
                        Id = Guid.NewGuid(),
                        UserName = user.UserName,
                        UserId = userId,
                        DeviceId = deviceId,
                        TimeStart = request.TimeStart,
                        TimeEnd = request.TimeEnd,
                        FaceImage = user.FaceImage,
                        CreatedAt = DateTime.UtcNow,
                    };

                    var rpcParamsDto = new RcpParamsDTO()
                    {
                        UserName = user.UserName,
                        UserId = userId,
                        Endtime = request.TimeEnd,
                        StartTime = request.TimeStart,
                        FaceImage = user.FaceImage,
                        FingerPrintImage = user.FingerprintImage,
                        IdNumber = user.IdNumber, //CCCD
                    };
                    var rpcRequestDto = new RcpRequestDTO()
                    {
                        Token = request.Token,
                        DeviceId = deviceId,
                        Method = "userSchedule",
                        Params = rpcParamsDto
                    };

                    var response = await _rcpService.SendRpcRequestAsync(rpcRequestDto);

                    await _permissionRepository.AddPermissionAsync(permission);
                    createdPermissions.Add(permission);
                }
            }
            return createdPermissions;
        }

        public async Task UpdatePermission(Guid PermisionId, PermissionModel permission)
        {
            var existingPermission = await _permissionRepository.GetPermissionsByIdAsync(PermisionId);

            if (existingPermission == null)
            {
                throw new Exception("Permissions info not found");
            }

            existingPermission.UserId = permission.UserId;
            existingPermission.DeviceId = permission.DeviceId;
            existingPermission.TimeStart = permission.TimeStart;
            existingPermission.TimeEnd = permission.TimeEnd;
            existingPermission.CreatedAt = permission.CreatedAt;

            await _permissionRepository.UpdatePermissionAsync(existingPermission);

        }

        public async Task DeletePermission(Guid PermisionId)
        {
            var existingPermission = await _permissionRepository.GetPermissionsByIdAsync(PermisionId);

            if (existingPermission == null)
            {
                throw new Exception("Permissions info not found");
            }

            await _permissionRepository.DeletePermissionAsync(PermisionId);
        }
    }
}
