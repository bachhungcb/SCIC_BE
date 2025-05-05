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

        public StudentService(IStudentInfoRepository studentRepository,
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

        public async Task<StudentDTO> GetStudentByIdAsync(Guid studentId)
        {
            var student = await _studentInfoRepository.GetByStudentIdAsync(studentId);
            var studentDTO = new StudentDTO
            {
                UserId = student.User.Id,
                UserName = student.User.Name,
                Email = student.User.Email,
                StudentCode = student.StudentCode,
                EnrollDate = student.EnrollDate,
            };
            return studentDTO;
        }

        public async Task CreateStudentAsync(CreateStudentDTO dto)
        {
            
            var user = await _userRepository.GetUserByIdAsync(dto.UserId);

            if (user == null)
            {

                throw new Exception("User not found");
            }

            var studentRole = await _roleRepository.GetRoleByNameAsync("Student");
            if (studentRole == null)
            {
                
                throw new Exception("Role 'Student' not found");
            }

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
                await CreateStudentInfoAsync(dto.UserId, dto.StudentCode, dto.EnrollDate);
            }
        }


        public async Task<List<StudentDTO>> GetListStudentAsync()
        {
            var students = await _studentInfoRepository.GetAllStudentsAsync(); //StudentModel

            var studentDTOs = students.Select(student => new StudentDTO
            {
                UserId = student.UserId,
                UserName = student.User.Name,
                Email = student.User.Email,
                StudentCode = student.StudentCode,
                EnrollDate = student.EnrollDate,
            }).ToList();
            return studentDTOs;
        }

        public async Task CreateStudentInfoAsync(Guid userId, string studentCode, DateTime enrollDate)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            var studentInfo = new StudentModel
            {
                UserId = userId,
                StudentCode = studentCode,
                EnrollDate = enrollDate,
                User = user
            };

            await _studentInfoRepository.AddAsync(studentInfo);
        }

        public async Task UpdateStudentInfoAsync(Guid userId, string newStudentCode)
        {
            var studentInfo = await _studentInfoRepository.GetByStudentIdAsync(userId);

            if (studentInfo == null)
                throw new Exception("Student info not found");

            studentInfo.StudentCode = newStudentCode;
           
            await _studentInfoRepository.UpdateAsync(studentInfo);
        }

        public async Task DeleteStudentAsync(Guid userId)
        {
            var studentInfo = await _studentInfoRepository.GetByStudentIdAsync(userId);

            if (studentInfo == null)
                throw new Exception("Student info not found");
            var user = await _userRepository.GetUserByIdAsync(userId);
            user.UserRoles.Clear();
            await _studentInfoRepository.DeleteAsync(studentInfo);
        }

    }

}
