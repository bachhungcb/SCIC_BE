using SCIC_BE.DTO.UserDTO;
using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices
{
    public interface IUserService
    {
        Task CreateUserAsync(CreateUserDTO dto);
        Task<UserModel> GetUserAsync(Guid id);
        Task<List<UserModel>> GetListUserAsync();
    }
}
