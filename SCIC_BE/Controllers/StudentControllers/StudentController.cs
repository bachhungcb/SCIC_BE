using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.DTO.StudentDTOs;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Services;
using SCIC_BE.Models;
using SCIC_BE.Helper;


namespace SCIC_BE.Controllers.StudentControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Default User, Student")]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
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

    }

}
