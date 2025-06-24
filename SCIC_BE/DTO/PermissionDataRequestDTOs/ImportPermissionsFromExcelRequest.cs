using Microsoft.AspNetCore.Http;

namespace SCIC_BE.DTO.PermissionDataRequestDTOs
{
    public class ImportPermissionsFromExcelRequest
    {
        public IFormFile ExcelFile { get; set; }
        public string Token { get; set; }
    }
}
