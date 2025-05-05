using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCIC_BE.DTO.LecturerDTOs;
using SCIC_BE.Helper;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Services;

namespace SCIC_BE.Controllers.LecturerControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerService _lecturerService;

        public LecturerController(ILecturerService lecturerService)
        {
            _lecturerService = lecturerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListLecturer()
        {
            var lecturers = await _lecturerService.GetListLecturerAsync();

            if (lecturers == null || !lecturers.Any())
            {
                return NotFound(ApiErrorHelper.Build(404, "No Lecturers found", HttpContext));
            }

            return Ok(lecturers);
        }

        [HttpGet("lecturer/{id}")]
        public async Task<IActionResult> GetLecturerById(Guid id)
        {
            var lecturer = await _lecturerService.GetLecturerByIdAsync(id);

            if (lecturer == null)
            {
                return NotFound(ApiErrorHelper.Build(404, $"lecturer with ID {id} not found", HttpContext));
            }

            return Ok(lecturer);
        }

        [HttpPost("create-lecturer/{id}")]
        public async Task<IActionResult> CreateLecturer(Guid id, [FromBody] CreateLecturerDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiErrorHelper.Build(400, "Bad Request", HttpContext));
            }

            try
            {
                await _lecturerService.CreateLecturerAsync(id, dto);
                return Ok(new { message = "Lecturer created successfully" });
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

        [HttpPut("update-lecturer/{id}")]
        public async Task<IActionResult> UpdateLecturer(Guid id, [FromBody] UpdateLecturerDTO dto)
        {
            try
            {
                await _lecturerService.UpdateLecturerInfoAsync(id, dto.NewLecturereCode);
                return Ok(new { message = "Lecturer updated successfully" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiErrorHelper.Build(404, $"Lecturer with ID {id} not found", HttpContext));
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

        [HttpDelete("delete-lecturer/{id}")]
        public async Task<IActionResult> DeleteLecturer(Guid id)
        {
            try
            {
                await _lecturerService.DeleteLecturerAsync(id);
                return Ok(new { message = "Lecturer deleted successfully" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiErrorHelper.Build(404, $"Lecturer with ID {id} not found", HttpContext));
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
