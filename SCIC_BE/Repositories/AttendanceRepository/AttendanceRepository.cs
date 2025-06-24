using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.Models;


namespace SCIC_BE.Repositories.AttendanceRepository
{
    public class AttendanceRepository :IAttendanceRepository
    {
        private readonly ScicDbContext _context;

        public AttendanceRepository(ScicDbContext context)
        {
            _context = context;
        }

        public async Task<List<AttendanceModel>> GetAllAttendanceAsync()
        {
            var attendances = await _context.Attendances.ToListAsync();
            return attendances;
        }

        public async Task<AttendanceModel> GetByAttendanceIdAsync(Guid id)
        {
            var attendance = await _context.Attendances.FirstOrDefaultAsync(att => att.Id == id);

            return attendance;
        }

        public async Task AddAsync(AttendanceModel model)
        {
            _context.Attendances.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AttendanceModel model)
        {
            _context.Attendances.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var attendance = await GetByAttendanceIdAsync(id);

            if (attendance == null)
            {
                throw new KeyNotFoundException("Attenance not found to delete");
            }
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStudentAttentAsync(Guid attendanceId)
        {
            var attendances = await GetByAttendanceIdAsync(attendanceId);
            if (attendances == null)
                throw new KeyNotFoundException("Attendance not found");
            
            attendances.IsAttended = true;
            // Tìm student cụ thể trong danh sách
            await _context.SaveChangesAsync();            
        }

    }
}
