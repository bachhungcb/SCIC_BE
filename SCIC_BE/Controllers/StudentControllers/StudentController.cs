using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Services;


namespace SCIC_BE.Controllers.StudentControllers
{
    [Route("/api/v1/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly UserInfoService _userInfoService;

        public StudentController(StudentService studentService, UserInfoService userInfoService)
        {
            _studentService = studentService;
            _userInfoService = userInfoService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetListStudentAsync()
        {
            var studentList = await _studentService.GetListStudentAsync();
            if (studentList == null || studentList.Count == 0)
            {
                return NotFound("No students found");
            }

            return Ok(studentList);
        }

        // Tạo thông tin sinh viên
        [HttpPost("create-student")]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDTO dto)
        {

            await _studentService.CreateStudentAsync(dto);

            return Ok();
        }

        // Cập nhật thông tin sinh viên
        [HttpPut("update-student")]
        public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentDTO dto)
        {
            await _studentService.UpdateStudentInfoAsync(dto.UserId, dto.NewStudentCode);
            return Ok();
        }



    }
}
