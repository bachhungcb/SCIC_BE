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
        Task<List<AttendanceDTO>> GetListAttendanceAsync();
        Task<AttendanceDTO> GetAttendanceByIdAsync(Guid id);
        Task<AttendanceDTO> GetAttendanceByDeviceIdAsync(Guid deviceId);
        Task<AttendanceDTO> GetAttendanceByStudentIdAsync(Guid studentId);
        Task<List<AttendanceDTO>> GetAttendancesByDeviceIdTodayAsync(Guid deviceId);
        Task<CreateAttendanceRCPDto> CreateAttendanceAsync(CreateAttendanceDTO attendanceInfo);
        Task<List<AttendanceModel>> UpdateAttendanceAsync(Guid id, UpdateAttendanceDTO updateInfo);
        Task UpdateStudentAttendancAsync(Guid deviceId, Guid studentId, string status);
        Task DeleteAttendanceAsync(Guid attendanceid);
    }
}
