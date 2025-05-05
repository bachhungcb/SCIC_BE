using SCIC_BE.DTO.LecturerDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.LecturerRepository
{
    public interface ILecturerRepository
    {
        Task<List<LecturerDTO>> GetAllLecturerAsync();
        Task<LecturerDTO> GetLecturerByIdAsync(Guid id);
        Task AddAsync(LecturerModel lecturerInfo);
        Task UpdateAsync(LecturerModel lecturerInfo);
        Task DeleteAsync(LecturerModel lecturerInfo);
    }
}
