using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.AttendanceLogRepository;

public interface IAttendanceLogRepository
{
    Task AddAttendanceLogAsync(AttendanceLogModel attendanceLog);
    Task<List<AttendanceLogModel>> GetAllAttendanceAsync();
    Task<AttendanceLogModel> GetAttendanceByIdAsync(Guid id);
    
}