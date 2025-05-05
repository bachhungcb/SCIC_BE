using SCIC_BE.DTO.RoleDTOs;


namespace SCIC_BE.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        Task<List<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> GetRoleByNameAsync(string name);
        Task AddRoleAsync(RoleDTO roleDTO);
    }
}
