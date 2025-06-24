using Microsoft.AspNetCore.Http;

namespace SCIC_BE.DTO.DeviceDTOs
{
    public class ImportDevicesFromExcelDTO
    {
        public IFormFile ExcelFile { get; set; }
    }
}
