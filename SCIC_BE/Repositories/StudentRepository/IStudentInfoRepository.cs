using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.Models;

namespace SCIC_BE.Repository.StudentRepository
{
    public interface IStudentInfoRepository
    {
        Task<List<StudentDTO>> GetAllStudentsAsync();
        Task<StudentModel> GetByStudentIdAsync(Guid id);
        Task AddAsync(StudentModel studentInfo);
        Task UpdateAsync(StudentModel studentInfo);
        Task DeleteAsync(StudentModel student);
    }

}
