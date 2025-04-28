using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.UserRepository;
using SCIC_BE.Repository.StudentRepository;

namespace SCIC_BE.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentInfoRepository _studentInfoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public StudentService(  IStudentInfoRepository studentRepository, 
                                IUserRepository userRepository,
                                IPasswordService passwordService)
        {
            _studentInfoRepository = studentRepository;
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task CreateStudentAsync(CreateStudentDTO dto)
        {
            // Kiểm tra xem User đã tồn tại hay chưa
            var user = await _userRepository.GetUserByIdAsync(dto.UserId);

            if (user == null)
            {
                // Nếu User không tồn tại, tạo mới một User
                user = new UserModel
                {
                    Id = dto.UserId,
                    Name = dto.Name,
                    Email = dto.Email,
                    PasswordHash = _passwordService.HashPassword(null, dto.Password)
                    // Thêm các thuộc tính khác của User nếu cần
                };
                await _userRepository.AddUserAsync(user);  // Giả sử có method này trong repository
            }

            // Tạo thông tin sinh viên (StudentInfo) sau khi chắc chắn User tồn tại
            var studentInfo = new StudentModel
            {
                UserId = dto.UserId,
                StudentCode = dto.StudentCode,
                EnrollDate = dto.EnrollDate
            };

            var existingStudentInfo = await _studentInfoRepository.GetByUserIdAsync(dto.UserId);

            if (existingStudentInfo == null)
            {
                // Lưu thông tin sinh viên vào cơ sở dữ liệu
                await CreateStudentInfoAsync(dto.UserId, dto.StudentCode, dto.EnrollDate);
            }
            else
            {
                existingStudentInfo.StudentCode = dto.StudentCode;
                existingStudentInfo.EnrollDate = dto.EnrollDate;
                await _studentInfoRepository.UpdateAsync(existingStudentInfo);
            }

        }

        public async Task<List<StudentDTO>> GetListStudentAsync()
        {
            var students = await _studentInfoRepository.GetAllStudentsAsync();

            // Nếu cần validate / xử lý thêm gì đó → xử lý ở đây

            return students;
        }

        public async Task CreateStudentInfoAsync(Guid userId, string studentCode, DateTime enrollDate)
        {
            var studentInfo = new StudentModel
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
