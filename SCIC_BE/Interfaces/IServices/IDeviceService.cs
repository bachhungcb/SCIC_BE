using Microsoft.AspNetCore.Mvc;
using SCIC_BE.DTO.DeviceDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices
{

    public interface IDeviceService
    {
        Task<List<DeviceModel>> ImportDevicesFromExcelAsync(ImportDevicesFromExcelDTO request);
    }
}
