using SCIC_BE.DTO.StudentDTOs;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.UserRepository;
using SCIC_BE.Repository.StudentRepository;

namespace SCIC_BE.Services
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
            var userRoleDTO = user.UserRoles.Select(role => new UserRoleDTO
            {
                RoleId = role.RoleId,
                RoleName = role.Role.Name,
            }).ToList();

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserRoles = userRoleDTO
            };
        }

        public async Task CreateUserAsync(CreateUserDTO dto)
        {
            var user = new UserModel
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
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
            return userList.Select(user => ConvertToUserDTO(user)).ToList();
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

            userInfo.Name = dto.Name;
            userInfo.Email = dto.Email;

            await _userRepository.UpdateUserAsync(userInfo);
        }
    }
}
