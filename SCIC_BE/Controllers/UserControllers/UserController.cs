using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.DTO.StudentDTOs;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Models;
using SCIC_BE.Repositories.UserRepository;
using SCIC_BE.Services;

namespace SCIC_BE.Controllers.UserControllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserInfoService _userInfoService;
        private readonly ScicDbContext _context;

        public UserController(UserInfoService userInfoService, ScicDbContext context)
        {
            _userInfoService = userInfoService;
            _context = context;
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
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody]UpdateUserDTO dto)
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

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync();

            var roleName = user?.UserRoles.FirstOrDefault()?.Role?.Name;

            return Ok(new
            {
                RoleName = roleName,
                RoleId = user?.UserRoles.FirstOrDefault()?.RoleId
            });
        }


    }
}
