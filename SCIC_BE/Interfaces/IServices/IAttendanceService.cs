using SCIC_BE.DTO.AttendanceDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices
{
    public interface IAttendanceService
    {
        Task<List<AttendanceDTO>> GetListAttendanceAsync();
        Task<AttendanceDTO> GetAttendanceByIdAsync(Guid id);
        Task<List<AttendanceModel>> CreateAttendanceAsync(CreateAttendanceDTO attendanceInfo);
        Task<List<AttendanceModel>> UpdateAttendanceAsync(Guid id, UpdateAttendanceDTO updateInfo);
        Task DeleteAttendanceAsync(Guid id);
    }
}
