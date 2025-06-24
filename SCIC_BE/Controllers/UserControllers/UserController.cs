using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.DTO.StudentDTOs;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.UserRepository;

namespace SCIC_BE.Controllers.UserControllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userInfoService;

        public UserController(IUserService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userInfoService.GetUserAsync(id);
            return Ok(user);
        }

        [HttpGet("get-defualt-user")]
        public async Task<IActionResult> GetUserWithDefaultRole()
        {
            var userList = await _userInfoService.GetListUserWithDefaultRoleAsync();
            return Ok(userList);
        }
       

    }
}
