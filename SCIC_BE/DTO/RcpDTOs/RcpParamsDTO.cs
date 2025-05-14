using Microsoft.EntityFrameworkCore;

namespace SCIC_BE.DTO.RcpDTOs
{
    public class RcpParamsDTO
    {
        public Guid userId { get; set; }
        public required string username { get; set; }
        public required string identifyNumber { get; set; } //CCCD 
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public required string faceImage { get; set; }
        public required string FingerPrintImage {  get; set; }

    }
}
