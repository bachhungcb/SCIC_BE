using SCIC_BE.Models;

namespace SCIC_BE.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task AddUserAsync(UserModel user);
        Task<UserModel> GetUserByIdAsync(Guid id);
        Task<List<UserModel>> GetAllUsersAsync();

    }
}
