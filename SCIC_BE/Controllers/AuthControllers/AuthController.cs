using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SCIC_BE.DTO.AuthDTOs;
using SCIC_BE.DTO.RoleDTOs;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Models;
using SCIC_BE.Repositories.RoleRepository;
using SCIC_BE.Repositories.UserRepository;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SCIC_BE.Services.Server;


namespace SCIC_BE.Controllers.AuthControllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserModel> _passwordHasher;
        private readonly JwtService _jwtService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public AuthController(
        IUserRepository userRepository,
        IPasswordHasher<UserModel> passwordHasher,
        JwtService jwtService,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDTO dto)
        {

            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            var user = new UserModel
            {
                Id = Guid.NewGuid(),
                IdNumber = dto.IdNumber,
                UserName = dto.UserName,
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(null, dto.Password),
                FaceImage = dto.FaceImage,
                FingerprintImage = dto.FingerprintImage,
            };

            await _userRepository.AddUserAsync(user);

            var defaultRole = await _roleRepository.GetRoleByNameAsync("Default User");

            if (defaultRole == null)
            {
                defaultRole = new RoleDTO
                {
                    Id = 4,
                    Name = "Default User"
                };
                await _roleRepository.AddRoleAsync(defaultRole);
            }

            var userRole = new UserRoleModel
            {
                UserId = user.Id,
                RoleId = 4 //Default User is 4
            };

            await _userRoleRepository.AddAsync(userRole);

            return Ok("User registered successfully with Default role");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var result = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, dto.Password);
            if (result != PasswordVerificationResult.Success)
                return Unauthorized(new { message = "Invalid credentials" });

            var roles = user.UserRoles?
                            .Where(ur => ur.Role != null)
                            .Select(ur => ur.Role.Name)
                            .ToList() ?? new List<string>();


            var token = _jwtService.GenerateToken(user, roles);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                roles,
                Token = token
            });
        }

        [HttpPut("change-password/{id}")]
        public async Task<IActionResult> ChangePassword(Guid id, string newPassword)
        {
            try
            {
                var user = await _userRepository.GetUserEntityByIdAsync(id);

                if (user == null)
                    return BadRequest(new { message = "User not found" });

                var newHashedPassword = _passwordHasher.HashPassword(null, newPassword);

                user.PasswordHash = newHashedPassword;

                await _userRepository.UpdateUserAsync(user);

                return Ok(new
                {
                    message = "Changed password successfully"
                });
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

        [HttpPut("forgot-password/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string email, string newPassword)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user == null)
                    return Unauthorized(new { message = "Invalid credentials" });

                var newHashedPassword = _passwordHasher.HashPassword(null, newPassword);

                user.PasswordHash = newHashedPassword;

                await _userRepository.UpdateUserAsync(user);

                return Ok(new
                {
                    message = "Changed password successfully"
                });

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
