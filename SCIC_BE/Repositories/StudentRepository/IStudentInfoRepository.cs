using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.Models;

namespace SCIC_BE.Repository.StudentRepository
{
    public interface IStudentInfoRepository
    {
        Task<List<StudentDTO>> GetAllStudentsAsync();
        Task<StudentInfoModel> GetByUserIdAsync(Guid id);
        Task AddAsync(StudentInfoModel studentInfo);
        Task UpdateAsync(StudentInfoModel studentInfo);
        //Task DeleteAsync(Guid id);
    }

}
