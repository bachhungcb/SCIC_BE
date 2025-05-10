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
    [Authorize(Roles = "Student, Lecturer")]
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerService _lecturerService;

        public LecturerController(ILecturerService lecturerService)
        {
            _lecturerService = lecturerService;
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

        


    }
}
