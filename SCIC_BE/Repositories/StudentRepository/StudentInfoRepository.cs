using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.Models;

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
            var students =  await _context.Set<StudentModel>().Include(s => s.User).ToListAsync();
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

        public async Task<StudentModel> GetByUserIdAsync(Guid id)
        {
            return await _context.Student.FirstOrDefaultAsync(s => s.UserId == id);
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


    }

}
