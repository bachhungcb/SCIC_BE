using Microsoft.AspNetCore.Mvc;
using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.Models;
using SCIC_BE.Repositories.UserRepository;
using SCIC_BE.Services;

namespace SCIC_BE.Controllers.UserInfoControllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserInfoController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly UserInfoService _userInfoService;

        public UserInfoController(StudentService studentService, UserInfoService userInfoService)
        {
            _studentService = studentService;
            _userInfoService = userInfoService;
        }

        // Tạo thông tin sinh viên
        [HttpPost("student")]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDTO dto)
        {
           
            await _userInfoService.CreateStudentAsync(dto);

            return Ok();
        }

        // Cập nhật thông tin sinh viên
        [HttpPut("student")]
        public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentDTO dto)
        {
            await _studentService.UpdateStudentInfoAsync(dto.UserId, dto.NewStudentCode);
            return Ok();
        }

        [HttpGet("student/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userInfoService.GetUserAsync(id);
            return Ok(user);
        }

    }
}
