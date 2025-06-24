using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.DTO.StudentDTOs;
using SCIC_BE.Models;
using SCIC_BE.Repositories.UserRepository;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SCIC_BE.Repository.StudentRepository
{
    public class StudentInfoRepository : IStudentInfoRepository
    {
        private readonly ScicDbContext _context;

        public StudentInfoRepository(ScicDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentModel>> GetAllStudentsAsync()
        {
            var students = await _context.Student
                                         .Include(s => s.User)  // Bao gồm thông tin User
                                         .ToListAsync();
            return students;
        }

        public async Task<StudentModel> GetByStudentIdAsync(Guid id)
        {
            var student = await _context.Student
                                         .Include(s => s.User)  // Bao gồm thông tin User
                                         .ThenInclude(u => u.UserRoles)  // Bao gồm các UserRoles
                                         .ThenInclude(ur => ur.Role)  // Bao gồm Role của User
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
            // Kiểm tra nếu học sinh có tồn tại trong cơ sở dữ liệu
            var existingStudent = await _context.Student.FirstOrDefaultAsync(s => s.UserId == studentInfo.UserId);
            if (existingStudent == null)
            {
                throw new KeyNotFoundException("Student not found to update");
            }

            _context.Student.Update(studentInfo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(StudentModel student)
        {
            var existingStudent = await _context.Student.FirstOrDefaultAsync(s => s.UserId == student.UserId);
            if (existingStudent == null)
            {
                throw new KeyNotFoundException("Student not found to delete");
            }

            _context.Student.Remove(existingStudent);
            await _context.SaveChangesAsync();
        }


    }

}
