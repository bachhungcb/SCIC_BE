using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repository.StudentRepository;

namespace SCIC_BE.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentInfoRepository _studentInfoRepository;

        public StudentService(IStudentInfoRepository studentRepository)
        {
            _studentInfoRepository = studentRepository;
        }

        public async Task<List<StudentDTO>> GetListStudentAsync()
        {
            var students = await _studentInfoRepository.GetAllStudentsAsync();

            // Nếu cần validate / xử lý thêm gì đó → xử lý ở đây

            return students;
        }

        public async Task CreateStudentInfoAsync(Guid userId, string studentCode, DateTime enrollDate)
        {
            var studentInfo = new StudentInfoModel
            {
                UserId = userId,
                StudentCode = studentCode,
                EnrollDate = enrollDate
            };

            await _studentInfoRepository.AddAsync(studentInfo);
        }

        public async Task UpdateStudentInfoAsync(Guid userId, string newStudentCode)
        {
            var studentInfo = await _studentInfoRepository.GetByUserIdAsync(userId);

            if (studentInfo == null)
                throw new Exception("Student info not found");

            studentInfo.StudentCode = newStudentCode;
            await _studentInfoRepository.UpdateAsync(studentInfo);
        }
    }

}
