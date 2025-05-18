using Microsoft.EntityFrameworkCore;

namespace SCIC_BE.DTO.RcpDTOs
{
    public class RcpParamsDTO
    {
        public Guid userId { get; set; }
        public Guid deviceId { get; set; }
        public Guid permissionId { get; set; }
        public string username { get; set; }
        public string identifyNumber { get; set; } //CCCD 
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string faceImage { get; set; }
        public string FingerPrintImage {  get; set; }

    }
}
