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

        public async Task<List<LecturerDTO>> GetAllLecturerAsync()
        {
            var lecturers = await _context.Set<LecturerModel>()
                                            .Include(s => s.User)
                                            .ToListAsync();
            var lectureDTOs = lecturers.Select(s => new LecturerDTO
            {
                UserId = s.UserId,
                UserName = s.User?.Name,
                Email = s.User?.Email,
                LecturerCode = s.LecturerCode,
                HireDate = s.HireDate,
            }).ToList();

            return lectureDTOs;
        }

        public async Task<LecturerDTO> GetLecturerByIdAsync(Guid id)
        {
            var lecturer = await _context.Lecturer
                            .FirstOrDefaultAsync(s => s.User.Id == id);
            if (lecturer == null)
            {
                return null;
            }

            var lecturerDTO = new LecturerDTO
            {
                UserId = lecturer.UserId,
                UserName = lecturer.User?.Name,
                Email = lecturer.User?.Email,
                LecturerCode = lecturer.LecturerCode,
                HireDate = lecturer.HireDate,
            };

            return lecturerDTO;
        }
        public async Task AddAsync(LecturerModel lecturerInfo)
        {
            _context.Lecturer.Add(lecturerInfo);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(LecturerModel lecturerInfo)
        {
            _context.Lecturer.Update(lecturerInfo);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(LecturerModel lecturerInfo)
        {
            _context.Lecturer.Remove(lecturerInfo);
            await _context.SaveChangesAsync();
        }

    }
}
