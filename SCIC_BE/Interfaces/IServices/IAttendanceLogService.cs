using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices;

public interface IAttendanceLogService
{
    Task AddAttendanceLogAsync(Guid userId, Guid deviceId, string status);
    Task<List<AttendanceLogModel>> GetAllAttendanceAsync();
    Task<AttendanceLogModel> GetAttendanceByIdAsync(Guid id);
}