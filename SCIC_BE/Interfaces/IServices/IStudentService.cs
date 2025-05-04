using Microsoft.AspNetCore.Mvc;
using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices
{
    public interface IStudentService
    {
        Task<List<StudentDTO>> GetListStudentAsync();
        Task CreateStudentAsync(CreateStudentDTO dto);
        Task CreateStudentInfoAsync(Guid userId, string studentCode, DateTime enrollDate);
        Task DeleteStudentAsync(Guid userId);
        Task<StudentModel> GetStudentByIdAsync(Guid studentId);
    }

}
