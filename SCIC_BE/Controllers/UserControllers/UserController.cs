using Microsoft.AspNetCore.Mvc;
using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.DTO.UserDTO;
using SCIC_BE.Models;
using SCIC_BE.Repositories.UserRepository;
using SCIC_BE.Services;

namespace SCIC_BE.Controllers.UserInfoControllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserInfoService _userInfoService;

        public UserController(UserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

       
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
        {
            await _userInfoService.CreateUserAsync(dto);

            return Ok();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userInfoService.GetUserAsync(id);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var userList = await _userInfoService.GetListUserAsync();
            return Ok(userList);
        }
    }
}
