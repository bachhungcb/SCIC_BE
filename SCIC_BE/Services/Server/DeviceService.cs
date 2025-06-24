using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using SCIC_BE.DTO.DeviceDTOs;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;

namespace SCIC_BE.Services.Server
{
    public class DeviceService : IDeviceService
    {
        private readonly IServiceCollection _services;
        public DeviceService(IServiceCollection services)
        {
            _services = services;
        }

        public async Task<List<DeviceModel>> ImportDevicesFromExcelAsync(ImportDevicesFromExcelDTO request)
        {
            if(request.ExcelFile == null || request.ExcelFile.Length <= 0) 
            {
                throw new Exception("File không được để trống");
            }

            if(!Path.GetExtension(request.ExcelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("File phải có định dạng .xlsx");
            }
            var devices = new List<DeviceModel>();

            using (var stream = new MemoryStream())
            {
                await request.ExcelFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    // Bỏ qua dòng header
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var device = new DeviceModel
                        {
                            Name = worksheet.Cells[row, 1].Value?.ToString() ?? "",
                            Type = worksheet.Cells[row, 2].Value?.ToString() ?? "",
                            Label = worksheet.Cells[row, 3].Value?.ToString() ?? "",
                            MacAddress = worksheet.Cells[row,4].Value?.ToString() ?? ""
                        };

                        // Validate dữ liệu
                        if (string.IsNullOrWhiteSpace(device.Name) ||
                            string.IsNullOrWhiteSpace(device.Type) ||
                            string.IsNullOrWhiteSpace(device.Label))
                        {
                            continue; // Bỏ qua dòng không hợp lệ
                        }

                        devices.Add(device);
                    }
                }
            }

            return devices;
        }
    }
}
