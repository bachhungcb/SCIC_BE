using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.AttendanceRepository
{
    public interface IAttendanceRepository
    {
        Task<List<AttendanceModel>> GetAllAttendanceAsync();
        Task<AttendanceModel> GetByAttendanceIdAsync(Guid id);
        Task AddAsync(AttendanceModel model);
        Task UpdateAsync(AttendanceModel model);
        Task DeleteAsync(Guid id);
        Task UpdateStudentAttentAsync(Guid attendanceId);

    }
}
