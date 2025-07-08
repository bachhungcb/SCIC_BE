using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.AttendanceLogRepository;

public class AttendanceLogRepository : IAttendanceLogRepository
{
    private readonly ScicDbContext _context;

    public AttendanceLogRepository(ScicDbContext context)
    {
        _context = context;
    }

    public async Task<List<AttendanceLogModel>> GetAllAttendanceAsync()
    {
        var logs = await _context.AttendanceLogs.ToListAsync();
        
        return logs;
    }

    public async Task<AttendanceLogModel> GetAttendanceByIdAsync(Guid id)
    {
        var log = await _context.AttendanceLogs.FirstOrDefaultAsync(l => l.Id == id);
        
        return log;
    }

    public async Task AddAttendanceLogAsync(AttendanceLogModel attendanceLog)
    {
        await _context.AttendanceLogs.AddAsync(attendanceLog);
        await _context.SaveChangesAsync();
    }
}