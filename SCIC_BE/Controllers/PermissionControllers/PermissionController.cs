using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCIC_BE.DTO.PermissionDataRequestDTOs;
using SCIC_BE.Helper;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;

namespace SCIC_BE.Controllers.PermissionControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet("list-permission")]
        public async Task<IActionResult> GetListPermission()
        {
            var permissions = await _permissionService.GetListPermissionsAsync();

            if (permissions == null || permissions.Count == 0)
            {
                return NotFound(ApiErrorHelper.Build(404, "No Permission found", HttpContext));
            }

            return Ok(permissions);
        }

        [HttpGet("get-permission/{id}")]
        public async Task<IActionResult> GetPermission(Guid id)
        {
            var permission = await _permissionService.GetPermissionByPermissonIdAsync(id);

            if (permission == null)
            {
                return NotFound(ApiErrorHelper.Build(404, "No Permission found", HttpContext));
            }

            return Ok(permission);
        }

        [HttpPost("create-permission")]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionDataRequestDTO request)
        {
            try
            {
                var permission = await _permissionService.CreatePermission(request);
                return Ok(new
                {
                    message = "Create Permission Success"
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

        [HttpPut("update-permission/{id}")]
        public async Task<IActionResult> UpdatePermission([FromBody] PermissionDataRequestDTO request, Guid id)
        {
            try
            {
                await _permissionService.UpdatePermission(id, request);
                return Ok(new { message = "Permission updated successfully" });
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

        [HttpDelete("delete-permission/{id}")]
        public async Task<IActionResult> DeletePermission(Guid id)
        {
            try
            {
                await _permissionService.DeletePermission(id);
                return Ok(new
                {
                    message = "Permission deleted successfully"
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
