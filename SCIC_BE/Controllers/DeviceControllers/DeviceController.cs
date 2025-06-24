using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SCIC_BE.DTO.DeviceDTOs;
using SCIC_BE.Interfaces.IServices;

namespace SCIC_BE.Controllers.DeviceControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpPost("import-excel")]
        public async Task<IActionResult> ImportDeviceFromExcel([FromForm] ImportDevicesFromExcelDTO dto)
        {
            try
            {
                var devices = await _deviceService.ImportDevicesFromExcelAsync(dto);
                return Ok(devices);
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
