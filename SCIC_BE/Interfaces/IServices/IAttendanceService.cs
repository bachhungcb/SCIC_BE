using SCIC_BE.DTO.AttendanceDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices
{
    public interface IAttendanceService
    {
        Task<List<AttendanceModel>> GetListAttendanceAsync();
        Task<AttendanceModel> GetAttendanceByIdAsync(Guid id);
        Task<AttendanceModel> CreateAttendanceAsync(CreateAttendanceDTO attendanceInfo);
        Task UpdateAttendanceAsync(Guid id, UpdateAttendanceDTO updateInfo);
        Task DeleteAttendanceAsync(Guid id);
    }
}
