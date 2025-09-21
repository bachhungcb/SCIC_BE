using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.DTO.AttendanceDTOs;
using SCIC_BE.DTO.RcpDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices
{
    public interface IAttendanceService
    {
        Task<List<AttendanceModel>> GetListAttendanceAsync();
        Task<AttendanceModel> GetAttendanceByIdAsync(Guid id);
        Task<AttendanceModel> GetAttendanceByDeviceIdAsync(Guid deviceId);
        Task<AttendanceModel> GetAttendanceByStudentIdAsync(Guid studentId);
        Task<List<AttendanceDTO>> GetAttendancesByDeviceIdTodayAsync(Guid deviceId);
        Task<CreateAttendanceRCPDto> CreateAttendanceAsync(CreateAttendanceDTO attendanceInfo);
        Task<AttendanceModel> UpdateAttendanceAsync(Guid id, UpdateAttendanceDTO updateInfo);
        Task UpdateStudentAttendancAsync(Guid deviceId, Guid studentId);
        Task DeleteAttendanceAsync(Guid attendanceid);
    }
}
