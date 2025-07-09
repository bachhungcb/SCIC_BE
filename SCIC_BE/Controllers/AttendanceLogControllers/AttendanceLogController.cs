using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SCIC_BE.Interfaces.IServices;

namespace SCIC_BE.Controllers.AttendanceLogControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AttendanceLogController : ControllerBase
    {
        private readonly IAttendanceLogService _attendanceLogService;

        public AttendanceLogController(IAttendanceLogService attendanceLogService)
        {
            _attendanceLogService = attendanceLogService;
        }

        [HttpGet("log/{id:guid}")]
        public async Task<IActionResult> GetLogById(Guid id)
        {
            try
            {
                var log = await _attendanceLogService.GetAttendanceByIdAsync(id);
                
                return Ok(log);
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

        [HttpGet("list-log")]
        public async Task<IActionResult> GetListLog()
        {
            try
            {
                var logs = await _attendanceLogService.GetAllAttendanceAsync();
                
                return Ok(logs);
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
