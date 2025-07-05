using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.UserRepository;

namespace SCIC_BE.Services.Server
{
    public class UserInfoService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        public UserInfoService(IUserRepository userRepository,
                                IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
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
                //FaceImage = user.FaceImage,
                //FingerprintImage = user.FingerprintImage,
                UserRoles = user.UserRoles?.Select(role => role.Role?.Name).Where(name => name != null).ToList()
            };
        }


        public async Task CreateUserAsync(CreateUserDTO dto)
        {
            var user = new UserModel
            {
                Id = Guid.NewGuid(),
                IdNumber = dto.IdNumber,
                UserName = dto.UserName,
                FullName = dto.FullName,
                Email = dto.Email,
                FaceImage = dto.FaceImage,
                FingerprintImage = dto.FingerprintImage,
                PasswordHash = _passwordService.HashPassword(null, dto.Password)

            };

            await _userRepository.AddUserAsync(user);

        }


        public async Task<UserDTO> GetUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return ConvertToUserDTO(user);
        }


        public async Task<List<UserDTO>> GetListUserAsync()
        {
            var userList = await _userRepository.GetAllUsersAsync();

            if (userList == null)
            {
                return null;
            }
            return userList.ToList();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var userInfo = await _userRepository.GetUserByIdAsync(id);
            if (userInfo == null)
            {
                throw new Exception("User info not found");
            }
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task UpdateUserAsync(Guid id, UpdateUserDTO dto)
        {
            var userInfo = await _userRepository.GetUserByIdAsync(id);

            if (userInfo == null)
            {
                throw new Exception("User info not found");
            }

            userInfo.UserName = dto.Name;
            userInfo.Email = dto.Email;

            await _userRepository.UpdateUserAsync(userInfo);
        }

        public async Task<List<UserDTO>> GetListUserWithDefaultRoleAsync()
        {
            var userList = await _userRepository.GetAllUsersAsync();
            var userListWithoutRole = userList
                                        .Where(user => user.UserRoles.Count == 1 & user.UserRoles.Any(role => role.Equals("Default User")));
            
            return userListWithoutRole.Select(user => user).ToList();
           
        }
    }
}
