using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices
{
    public interface IUserService
    {
        Task CreateUserAsync(CreateUserDTO dto);
        Task<UserDTO> GetUserAsync(Guid id);
        Task<List<UserDTO>> GetListUserAsync();
        Task UpdateUserAsync(Guid id, UpdateUserDTO dto);
        Task DeleteUserAsync(Guid id);
    }
}
