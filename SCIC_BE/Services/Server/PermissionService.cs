using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCIC_BE.DTO.PermissionDataRequestDTOs;
using SCIC_BE.DTO.RcpDTOs;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.PermissionRepository;
using SCIC_BE.Repositories.RoleRepository;
using SCIC_BE.Repositories.UserRepository;

namespace SCIC_BE.Services.Server
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly RcpService _rcpService;
        private readonly IRoleRepository _roleRepository;
        public PermissionService(IPermissionRepository permissionRepository,
                                 IUserRepository userRepository,
                                 RcpService rcpService,
                                 IRoleRepository roleRepository)
        {
            _permissionRepository = permissionRepository;
            _userRepository = userRepository;
            _rcpService = rcpService;
            _roleRepository = roleRepository;
        }
        private UserDTO ConvertToUserDTO(UserModel user)
        {
            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                IdNumber = user.IdNumber,
                Email = user.Email,
                FaceImage = user.FaceImage,
                FingerprintImage = user.FingerprintImage,
                UserRoles = user.UserRoles?.Select(role => role.Role?.Name).Where(name => name != null).ToList()
            };
        }


        public async Task<List<PermissionDTO>> GetListPermissionsAsync()
        {
            var permissions = await _permissionRepository.GetAllPermissionsDtoAsync();
            return permissions;
        }

        public async Task<PermissionDTO> GetPermissionByPermissonIdAsync(Guid id)
        {
            var permission = await _permissionRepository.GetPermissionDToByIdAsync(id);

            return permission;
        }

        public async Task<PermissionModel> CreatePermission(PermissionDataRequestDTO request)
        {
            var permission = new PermissionModel
            {
                Id = Guid.NewGuid(),
                TimeStart = request.TimeStart,
                TimeEnd = request.TimeEnd,
                CreatedAt = DateTime.Now,
                DeviceId = request.DeviceIds
            };

            var permissionUsers = new List<PermissionUser>();
            var rpcUserDtOs = new List<UserDTO>();

            foreach (var userId in request.UserIds)
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                    throw new Exception($"User with ID {userId} not found.");

                rpcUserDtOs.Add(user);

                permissionUsers.Add(new PermissionUser
                {
                    PermissionId = permission.Id,
                    UserId = user.Id
                });
            }

            permission.PermissionUsers = permissionUsers;

            // Gửi RPC cho từng thiết bị
            var rpcParamsDto = new RcpParamsDTO
            {
                Users = rpcUserDtOs,
                DeviceId = request.DeviceIds,
                TimeStart = request.TimeStart,
                TimeEnd = request.TimeEnd,
                CreatedAt = DateTime.Now
            };

            foreach (var deviceId in request.DeviceIds)
            {
                var rpcRequestDto = new RcpRequestDTO
                {
                    DeviceId = deviceId,
                    Method = "createPermission",
                    Params = rpcParamsDto
                };

                try
                {
                    await _rcpService.SendRpcRequestAsync(rpcRequestDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"RPC error: {ex.Message}");
                    throw new Exception($"RPC error: {ex.Message}");
                }
            }

            // Lưu Permission nếu RPC thành công
            await _permissionRepository.AddPermissionAsync(permission);

            return permission;
        }



        public async Task<PermissionModel> UpdatePermission(Guid PermissionId ,PermissionDataRequestDTO request)
        {
            var updatedPermissions = new List<PermissionModel>();
            var existingPermission = await _permissionRepository.GetPermissionsByIdAsync(PermissionId);
            var permissionUsers = new List<PermissionUser>();
            var rpcUserDTOs = new List<UserDTO>();
            foreach (var userId in request.UserIds)
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                    throw new Exception($"User with ID {userId} not found.");

                rpcUserDTOs.Add(user);

                permissionUsers.Add(new PermissionUser
                {
                    PermissionId = existingPermission.Id,
                    UserId = user.Id
                });
            }

            existingPermission.PermissionUsers = permissionUsers;

            // Gửi RPC cho từng thiết bị
            var rpcParamsDto = new RcpParamsDTO
            {
                Users = rpcUserDTOs,
                DeviceId = request.DeviceIds,
                TimeStart = request.TimeStart,
                TimeEnd = request.TimeEnd,
                CreatedAt = DateTime.Now
            };

            foreach (var deviceId in request.DeviceIds)
            {
                var rpcRequestDto = new RcpRequestDTO
                {
                    DeviceId = deviceId,
                    Method = "updatePermission",
                    Params = rpcParamsDto
                };

                try
                {
                    await _rcpService.SendRpcRequestAsync(rpcRequestDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"RPC error: {ex.Message}");
                    throw new Exception($"RPC error: {ex.Message}");
                }
            }

            // Lưu Permission nếu RPC thành công
            await _permissionRepository.UpdatePermissionAsync(existingPermission);

            return existingPermission;
        }


        public async Task DeletePermission(Guid PermisionId)
        {
            var existingPermission = await _permissionRepository.GetPermissionsByIdAsync(PermisionId);

            if (existingPermission == null)
            {
                throw new Exception("Permissions info not found");
            }

            var rcpParamDto = new DeletePermissionRcpDTO()
            {
                permissionId = PermisionId,
            };
            foreach (var deviceId in existingPermission.DeviceId)
            {
                var rcpRequestDto = new RcpRequestDTO()
                {
                    DeviceId = deviceId,
                    Method = "deletePermission",
                    Params = rcpParamDto
                };
                
                try
                {
                    // Gửi yêu cầu RPC
                    await _rcpService.SendRpcRequestAsync(rcpRequestDto);
                }
                catch (Exception ex)
                {
                    // Log thông tin chi tiết về lỗi
                    Console.WriteLine($"Error occurred while processing RPC request for userId and DeviceId: {ex.Message}");
                    throw new Exception($"Error occurred while processing RPC request for userId and DeviceId");
                }
                
            }
            
            await _permissionRepository.DeletePermissionAsync(PermisionId);
        }

    }
}
