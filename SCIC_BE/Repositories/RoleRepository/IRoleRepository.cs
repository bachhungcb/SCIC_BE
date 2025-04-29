using SCIC_BE.DTO.RoleDTO;


namespace SCIC_BE.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        Task<List<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> GetRoleByNameAsync(string name);
        Task AddRoleAsync(RoleDTO roleDTO);
    }
}
