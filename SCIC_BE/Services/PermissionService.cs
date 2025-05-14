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
            var response = "";

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
                        username = user.UserName,
                        userId = userId,
                        endTime = request.TimeEnd,
                        startTime = request.TimeStart,
                        faceImage = user.FaceImage,
                        FingerPrintImage = user.FingerprintImage,
                        identifyNumber = user.IdNumber, //CCCD
                    };

                    var rpcRequestDto = new RcpRequestDTO()
                    {
                        Token = request.Token,
                        DeviceId = deviceId,
                        Method = "userSchedule",
                        Params = rpcParamsDto
                    };

                    // Gửi yêu cầu RPC
                    try
                    {
                        // Gửi yêu cầu RPC
                        await _rcpService.SendRpcRequestAsync(rpcRequestDto);

                        // Cập nhật cơ sở dữ liệu nếu RPC thành công
                        await _permissionRepository.AddPermissionAsync(permission);
                        createdPermissions.Add(permission);
                    }
                    catch (Exception ex)
                    {
                        // Log thông tin chi tiết về lỗi
                        Console.WriteLine($"Error occurred while processing RPC request for userId {userId} and DeviceId {deviceId}: {ex.Message}");
                        throw new Exception($"Error occurred while processing RPC request for userId {userId} and DeviceId {deviceId}: {response}");
                    }

                }
            }

            return createdPermissions;
        }


        public async Task<List<PermissionModel>> UpdatePermission(Guid PermissionId ,PermissionDataRequestDTO request)
        {
            var updatedPermissions = new List<PermissionModel>();
            var response = "";

            // Duyệt qua từng userId
            foreach (var userId in request.UserIds)
            {
                var user = await _userRepository.GetUserByIdAsync(userId);

                // Duyệt qua từng DeviceId
                foreach (var deviceId in request.DeviceIds)
                {
                    // Lấy Permission đã tồn tại
                    var existingPermission = await _permissionRepository.GetPermissionsByIdAsync(PermissionId);

                    if (existingPermission == null)
                    {
                        throw new Exception($"Permission not found for userId: {userId} and DeviceId: {deviceId}");
                    }

                    // Cập nhật thông tin permission
                    existingPermission.UserName = user.UserName;
                    existingPermission.UserId = userId;
                    existingPermission.DeviceId = deviceId;
                    existingPermission.TimeStart = request.TimeStart;
                    existingPermission.TimeEnd = request.TimeEnd;
                    existingPermission.FaceImage = user.FaceImage;
                    existingPermission.CreatedAt = DateTime.UtcNow;

                    var rpcParamsDto = new RcpParamsDTO()
                    {
                        username = user.UserName,
                        userId = userId,
                        endTime = request.TimeEnd,
                        startTime = request.TimeStart,
                        faceImage = user.FaceImage,
                        FingerPrintImage = user.FingerprintImage,
                        identifyNumber = user.IdNumber, //CCCD
                    };
                    var rpcRequestDto = new RcpRequestDTO()
                    {
                        Token = request.Token,
                        DeviceId = deviceId,
                        Method = "userSchedule",
                        Params = rpcParamsDto
                    };

                    // Gửi RPC request
                    response = await _rcpService.SendRpcRequestAsync(rpcRequestDto);

                    // Cập nhật quyền trong cơ sở dữ liệu
                    await _permissionRepository.UpdatePermissionAsync(existingPermission);
                    updatedPermissions.Add(existingPermission);
                }
            }
            return updatedPermissions;
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
