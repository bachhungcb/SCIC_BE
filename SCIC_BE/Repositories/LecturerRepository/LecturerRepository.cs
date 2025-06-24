using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.DTO.LecturerDTOs;
using SCIC_BE.DTO.StudentDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.LecturerRepository
{
    public class LecturerRepository : ILecturerRepository
    {
        private readonly ScicDbContext _context;

        public LecturerRepository(ScicDbContext context)
        {
            _context = context;
        }

        public async Task<List<LecturerModel>> GetAllLecturerAsync()
        {
            var lecturers = await   _context.Set<LecturerModel>()
                                            .Include(s => s.User)
                                            .ToListAsync();

            return lecturers;
        }

        public async Task<LecturerModel> GetLecturerByIdAsync(Guid id)
        {
            var lecturer = await _context.Lecturer
                                         .Include(s => s.User)  // Bao gồm thông tin User
                                         .ThenInclude(u => u.UserRoles)  // Bao gồm các UserRoles
                                         .ThenInclude(ur => ur.Role)  // Bao gồm Role của User
                                         .FirstOrDefaultAsync(s => s.UserId == id);

            return lecturer;
        }
        public async Task AddAsync(LecturerModel lecturerInfo)
        {
            _context.Lecturer.Add(lecturerInfo);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(LecturerModel lecturerInfo)
        {
            var existingLecturer =  await _context.Lecturer.FirstOrDefaultAsync(s => s.UserId ==  lecturerInfo.UserId);

            if (existingLecturer == null)
            {
                throw new KeyNotFoundException("Lecturer not found to update");
            }


            _context.Lecturer.Update(lecturerInfo);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(LecturerModel lecturerInfo)
        {
            var existingLecturer = await _context.Lecturer.FirstOrDefaultAsync(s => s.UserId == lecturerInfo.UserId);

            if (existingLecturer == null)
            {
                throw new KeyNotFoundException("Lecturer not found to update");
            }


            _context.Lecturer.Remove(lecturerInfo);
            await _context.SaveChangesAsync();
        }

    }
}
