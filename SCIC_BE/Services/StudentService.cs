using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.RoleRepository;
using SCIC_BE.Repositories.UserRepository;
using SCIC_BE.Repository.StudentRepository;

namespace SCIC_BE.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentInfoRepository _studentInfoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public StudentService(  IStudentInfoRepository studentRepository,
                                IUserRepository userRepository,
                                IPasswordService passwordService,
                                IRoleRepository roleRepository,
                                IUserRoleRepository userRoleRepository)
        {
            _studentInfoRepository = studentRepository;
            _userRepository = userRepository;
            _passwordService = passwordService;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<StudentModel> GetStudentByIdAsync(Guid studentId)
        {
            var student = await _studentInfoRepository.GetByStudentIdAsync(studentId);
            return student;
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

            // Tìm role "Student"
            var studentRole = await _roleRepository.GetRoleByNameAsync("Student");
            if (studentRole == null)
            {
                return;
            }

            // Kiểm tra nếu User chưa có role đó
            var hasRole = user.UserRoles?.Any(ur => ur.RoleId == studentRole.Id) == true;
            if (!hasRole)
            {
                var userRole = new UserRoleModel
                {
                    UserId = user.Id,
                    RoleId = studentRole.Id
                };
                await _userRoleRepository.AddAsync(userRole);
            }


            var existingStudentInfo = await _studentInfoRepository.GetByStudentIdAsync(dto.UserId);

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
            var studentInfo = await GetStudentByIdAsync(userId);

            if (studentInfo == null)
                throw new Exception("Student info not found");

            studentInfo.StudentCode = newStudentCode;
            await _studentInfoRepository.UpdateAsync(studentInfo);
        }

        public async Task DeleteStudentAsync(Guid userId)
        {
            var studentInfo = await GetStudentByIdAsync(userId);

            if (studentInfo == null)
                throw new Exception("Student info not found");
            await _studentInfoRepository.DeleteAsync(studentInfo);
        }

    }

}
