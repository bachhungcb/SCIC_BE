using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.Models;
using System.Runtime.InteropServices;

namespace SCIC_BE.Repository.StudentRepository
{
    public class StudentInfoRepository : IStudentInfoRepository
    {
        private readonly ScicDbContext _context;

        public StudentInfoRepository(ScicDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentDTO>> GetAllStudentsAsync()
        {
            var students = await _context.Set<StudentModel>().Include(s => s.User).ToListAsync();
            var studentDTOs = students.Select(s => new StudentDTO
            {
                UserId = s.UserId,
                UserName = s.User?.Name,
                Email = s.User?.Email,
                StudentCode = s.StudentCode,
                EnrollDate = s.EnrollDate,
            }).ToList();

            return studentDTOs;
        }

        public async Task<StudentModel> GetByStudentIdAsync(Guid id)
        {
            var student = await _context.Student
                                        .Include(s => s.User)
                                        .ThenInclude(u => u.UserRoles)
                                        .ThenInclude(ur => ur.Role)
                                        .FirstOrDefaultAsync(s => s.UserId == id);

            return student;
        }

        public async Task AddAsync(StudentModel studentInfo)
        {
            _context.Student.Add(studentInfo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentModel studentInfo)
        {
            _context.Student.Update(studentInfo);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(StudentModel student)
        {
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
        }


    }

}
