using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCIC_BE.DTO.LecturerDTOs;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.LecturerRepository;
using SCIC_BE.Repositories.RoleRepository;
using SCIC_BE.Repositories.UserRepository;
using SCIC_BE.Repository.StudentRepository;

namespace SCIC_BE.Services.Server
{
    public class LecturerService : ILecturerService
    {
        private readonly ILecturerRepository _lecturerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IStudentInfoRepository _studentInfoRepository;

        public LecturerService(ILecturerRepository lecturerRepository,
                                IUserRepository userRepository,
                                IRoleRepository roleRepository,
                                IUserRoleRepository userRoleRepository,
                                IStudentInfoRepository studentInfoRepository)
        {
            _lecturerRepository = lecturerRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _studentInfoRepository = studentInfoRepository;
        }

        public async Task<List<LecturerDTO>> GetListLecturerAsync()
        {
            var lectureres = await _lecturerRepository.GetAllLecturerAsync();

            var lecturerDTOs = lectureres.Select(lecturer => new LecturerDTO
            {
                UserId = lecturer.UserId,
                UserName = lecturer.User.UserName,
                Email = lecturer.User.Email,
                LecturerCode = lecturer.LecturerCode,
                HireDate = lecturer.HireDate,
            }).ToList();

            return lecturerDTOs;
        }

        public async Task<LecturerDTO> GetLecturerByIdAsync(Guid lecturerId)
        {
            var lecturer = await _lecturerRepository.GetLecturerByIdAsync(lecturerId);
            var lecturerDTO = new LecturerDTO
            {
                UserId = lecturer.UserId,
                UserName = lecturer.UserName,
                Email = lecturer.Email,
                LecturerCode = lecturer.LecturerCode,
                HireDate = lecturer.HireDate
            };

            return lecturerDTO;
        }

        public async Task CreateLecturerAsync(Guid id, CreateLecturerDTO dto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var student = await _studentInfoRepository.GetByStudentIdAsync(id);

            if (student != null)
            {
                throw new Exception("Student can not be lecturer");
            }
            
            var lecturerRole = await _roleRepository.GetRoleByNameAsync("Lecturer");
            if (lecturerRole == null)
            {
                throw new Exception("Role 'Lecturer' not found");
            }

            var hasRole = user.UserRoles?.Any(ur => ur.Equals(lecturerRole)) == true;
            if (!hasRole)
            {
                var userRole = new UserRoleModel
                {
                    UserId = user.Id,
                    RoleId = lecturerRole.Id,
                };
                await _userRoleRepository.AddAsync(userRole);
            }

            await _userRoleRepository.RemoveDefaultRoleForUserAsync(id);
            var existingLecturerInfo = await _lecturerRepository.GetLecturerByIdAsync(id);
            if (existingLecturerInfo == null)
            {
                await CreateLecturerInfoAsync(id, dto.LecturerCode, dto.HireDate);
            }

        }


        public async Task CreateLecturerInfoAsync(Guid userId, string lecturerCode, DateTime hireDate)
        {
            var user = await _userRepository.GetUserEntityByIdAsync(userId);

            var lecturerInfo = new LecturerModel
            {
                UserId = userId,
                LecturerCode = lecturerCode,
                HireDate = hireDate,
                User = user
            };

            await _lecturerRepository.AddAsync(lecturerInfo);
        }

        public async Task DeleteLecturerAsync(Guid userId)
        {
            var lecturerInfo = await _lecturerRepository.GetLecturerByIdAsync(userId);

            if (lecturerInfo == null)
                throw new Exception("Lecturer info not found");

            var user = await _userRepository.GetUserByIdAsync(userId);

            var userRoles = user.UserRoles.ToList();
            foreach (var userRole in userRoles)
            {
                //await _userRoleRepository.RemoveAsync(userRole);
            }
            await _userRepository.DeleteUserAsync(userId);
        }

        public async Task UpdateLecturerInfoAsync(Guid userId, string lecturerCode)
        {
            var lecturerInfo = await _lecturerRepository.GetLecturerEntityByIdAsync(userId);

            if(lecturerInfo == null)
            {
                throw new Exception("Lecturer info not found");
            }

            lecturerInfo.LecturerCode = lecturerCode;
            
            await _lecturerRepository.UpdateAsync(lecturerInfo);
        }

    }
}
