using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SCIC_BE.DTO.AuthDTOs;
using SCIC_BE.DTO.RoleDTOs;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Models;
using SCIC_BE.Repositories.RoleRepository;
using SCIC_BE.Repositories.UserRepository;
using SCIC_BE.Services;
using System.Runtime.InteropServices;



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
        private bool IsBase64String(string base64String)
        {
            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                return false;
            }
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
                RoleId = defaultRole.Id
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

            return Ok(new {
                user.Id,
                user.UserName,
                user.Email,
                roles,
                Token = token });
        }

    }
}
