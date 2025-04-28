using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.DTO.UserDTO;
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
        public UserInfoService( IUserRepository userRepository,
                                IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
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

        
        public async Task<UserModel> GetUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<List<UserModel>> GetListUserAsync()
        {
            var userList = await _userRepository.GetAllUsersAsync();

            if(userList == null)
            {
                return null;
            }
            return userList;
        }

    }
}
