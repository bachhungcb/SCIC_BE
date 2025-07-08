using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCIC_BE.DTO.LecturerDTOs;
using SCIC_BE.DTO.StudentDTOs;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Helper;
using SCIC_BE.Interfaces.IServices;

namespace SCIC_BE.Controllers.AdminControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILecturerService _lecturerService;
        private readonly IUserService _userInfoService;

        public AdminController(
                                IStudentService studentService,
                                ILecturerService lecturerService,
                                IUserService userService)
        {
            _studentService = studentService;
            _lecturerService = lecturerService;
            _userInfoService = userService;
        }

        [HttpGet("list-student")]
        public async Task<IActionResult> GetListStudent()
        {
            var students = await _studentService.GetListStudentAsync();

            if (students == null || !students.Any())
            {
                return NotFound(ApiErrorHelper.Build(404, "No students found", HttpContext));
            }

            return Ok(students);
        }

        [HttpPost("create-student/{id}")]
        public async Task<IActionResult> CreateStudent(Guid id, [FromBody] CreateStudentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiErrorHelper.Build(400, "Bad Request", HttpContext));
            }

            try
            {
                await _studentService.CreateStudentAsync(id, dto);
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

        [HttpPut("update-student/{id}")]
        public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] UpdateStudentDTO dto)
        {
            try
            {
                await _studentService.UpdateStudentInfoAsync(id, dto.NewStudentCode);
                return Ok(new { message = "Student updated successfully" });
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

        [HttpGet("list-lecturer")]
        public async Task<IActionResult> GetListLecturer()
        {
            var lecturers = await _lecturerService.GetListLecturerAsync();

            if (lecturers == null || !lecturers.Any())
            {
                return NotFound(ApiErrorHelper.Build(404, "No Lecturers found", HttpContext));
            }

            return Ok(lecturers);
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

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
        {
            await _userInfoService.CreateUserAsync(dto);

            return Ok(new { message = "User created successfully" });
        }


        [HttpGet("list-user")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetAllUser()
        {
            var userList = await _userInfoService.GetListUserAsync();
            return Ok(userList);
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userInfoService.DeleteUserAsync(id);
                return Ok(new { message = "User deleted successfully" });
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

        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDTO dto)
        {
            try
            {
                await _userInfoService.UpdateUserAsync(id, dto);
                return Ok(new { message = "User updated successfully" });
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
