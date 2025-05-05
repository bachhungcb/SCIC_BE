using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Services;
using SCIC_BE.Models;
using SCIC_BE.Helper;


namespace SCIC_BE.Controllers.StudentControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly UserInfoService _userInfoService;

        public StudentController(StudentService studentService, UserInfoService userInfoService)
        {
            _studentService = studentService;
            _userInfoService = userInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListStudentAsync()
        {
            var students = await _studentService.GetListStudentAsync();

            if (students == null || !students.Any())
            {
                return NotFound(ApiErrorHelper.Build(404, "No students found", HttpContext));
            }

            return Ok(students);
        }

        [HttpGet("student/{id}")]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);

            if (student == null)
            {
                return NotFound(ApiErrorHelper.Build(404, $"Student with ID {id} not found", HttpContext));
            }

            return Ok(student);
        }

        [HttpPost("create-student")]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiErrorHelper.Build(400, "Bad Request", HttpContext));
            }

            try
            {
                await _studentService.CreateStudentAsync(dto);
                return Ok(new { message = "Student created successfully" });
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


        [HttpPut("update-student")]
        public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentDTO dto)
        {
            try
            {
                await _studentService.UpdateStudentInfoAsync(dto.UserId, dto.NewStudentCode);
                return Ok(new { message = "Student updated successfully" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiErrorHelper.Build(404, $"Student with ID {dto.UserId} not found", HttpContext));
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

        [HttpDelete("delete-student/{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            try
            {
                await _studentService.DeleteStudentAsync(id);
                return Ok(new { message = "Student deleted successfully" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiErrorHelper.Build(404, $"Student with ID {id} not found", HttpContext));
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
