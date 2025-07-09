using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.AttendanceLogRepository;

namespace SCIC_BE.Services.Server;

public class AttendanceLogService : IAttendanceLogService
{
    private readonly IAttendanceLogRepository _attendanceLogRepositoryrepository;

    public AttendanceLogService(IAttendanceLogRepository attendanceLogRepository)
    {
        _attendanceLogRepositoryrepository = attendanceLogRepository;
    }
    
    public async Task AddAttendanceLogAsync(Guid userId, Guid deviceId, string status)
    {
        var attendanceLog = new AttendanceLogModel
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DeviceId = deviceId,
            Status = status,
            CreatedOn = DateTime.Now,
        };
        
        await _attendanceLogRepositoryrepository.AddAttendanceLogAsync(attendanceLog);
    }

    public async Task<List<AttendanceLogModel>> GetAllAttendanceAsync()
    {
        var logs = await _attendanceLogRepositoryrepository.GetAllAttendanceAsync();
        return logs;   
    }

     public async Task<AttendanceLogModel> GetAttendanceByIdAsync(Guid id)
     {
         var log = await _attendanceLogRepositoryrepository.GetAttendanceByIdAsync(id);
         return log;
     }
}