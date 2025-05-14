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

        [HttpGet("attendance/{id}")]
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
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                var attendance = await _attendanceService.CreateAttendanceAsync(attendanceInfo);
                return Ok(new
                {
                    method = "userSchedule",
                    param = attendance,
                    token,
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
