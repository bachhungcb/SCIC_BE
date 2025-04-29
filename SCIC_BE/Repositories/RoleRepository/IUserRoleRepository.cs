using SCIC_BE.Models;

namespace SCIC_BE.Repositories.RoleRepository
{
    public interface IUserRoleRepository
    {
        Task AddAsync(UserRoleModel userRole);
        Task<List<string>> GetRolesByUserIdAsync(Guid userId);
        Task RemoveAsync(UserRoleModel userRole);
    }
}
