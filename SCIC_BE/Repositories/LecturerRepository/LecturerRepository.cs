using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
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
            var lecturers = await   _context.Lecturer
                                            .Include(s => s.User)
                                            .ToListAsync();

            return lecturers;
        }

        public async Task<LecturerModel> GetLecturerEntityByIdAsync(Guid id)
        {
            var lecturer = await _context.Lecturer.FirstOrDefaultAsync(u => u.UserId == id);
            // if (lecturer == null)
            //     throw new KeyNotFoundException("User not found");

            return lecturer;
        }
        
        public async Task<LecturerDTO> GetLecturerByIdAsync(Guid id)
        {
            var lecturer = await _context.Lecturer
                .Where(l => l.UserId == id)
                .Select(l => new LecturerDTO
                {
                    UserId = l.UserId,
                    LecturerCode = l.LecturerCode,
                    HireDate = l.HireDate,
                    // Thông tin từ bảng User
                    UserName = l.User!.UserName,
                    Email = l.User.Email,
                    FaceImage = l.User.FaceImage, //null
                    //FingerprintImage = null, // l.User.FingerprintImage,
                })
                .FirstOrDefaultAsync();
            
            return lecturer;
            
            // var lecturer = await _context.Lecturer
            //                             .Include(s => s.User)  // Bao gồm thông tin User
            //                             .ThenInclude(u => u.UserRoles)  // Bao gồm các UserRoles
            //                             .ThenInclude(ur => ur.Role)  // Bao gồm Role của User
            //                             .FirstOrDefaultAsync(s => s.UserId == id);
            //
            // return lecturer;
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
