using Microsoft.AspNetCore.Mvc;
using SCIC_BE.DTO.AttendanceDTOs;
using SCIC_BE.Interfaces.IServices;

namespace SCIC_BE.Controllers.AttendanceControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet("list-attendance")]
        public async Task<IActionResult> GetAllAttendanceSchedule()
        {
            try
            {
                var attendancesSchedules = await _attendanceService.GetListAttendanceAsync();

                return Ok(attendancesSchedules);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred",
                    details = ex.Message
                });
            }

        }

        [HttpGet("attendance/{id:guid}")]
        public async Task<IActionResult> GetAnAttendance(Guid id)
        {
            try
            {
                var attendance = await _attendanceService.GetAttendanceByIdAsync(id);
                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred",
                    details = ex.Message
                });
            }
        }

        [HttpPost("create-attendance")]
        public async Task<IActionResult> CreateAttendance([FromBody] CreateAttendanceDTO attendanceInfo)
        {
            try
            {
                var attendance = await _attendanceService.CreateAttendanceAsync(attendanceInfo);
                return Ok(new
                {
                    method = "createAttendance",
                    @param = attendance,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred",
                    details = ex.Message
                });
            }
        }

        [HttpPut("update-attendance/{id:guid}")]
        public async Task<IActionResult> UpdateAttendance(Guid id, [FromBody] UpdateAttendanceDTO attendanceInfo)
        {
            try
            {
                var updateAttendance = await _attendanceService.UpdateAttendanceAsync(id, attendanceInfo);
                return Ok(new
                {
                    method = "updateAttendance",
                    @param = updateAttendance,
                });
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred",
                    details = ex.Message
                });
            }
        }

        [HttpDelete("delete-attendance/{id:guid}")]
        public async Task<IActionResult> DeleteAttendance(Guid id)
        {
            try
            {
                await _attendanceService.DeleteAttendanceAsync(id);
                return Ok(new
                {
                    method = "deleteAttendance",
                    @param = id,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred",
                    details = ex.Message
                });
            }
        }
        
    }
}
