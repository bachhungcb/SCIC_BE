using Microsoft.EntityFrameworkCore;

namespace SCIC_BE.DTO.RcpDTOs
{
    public class RcpParamsDTO
    {
        public Guid UserId { get; set; }
        public required string UserName { get; set; }
        public required string IdNumber { get; set; } //CCCD 
        public DateTime StartTime { get; set; }
        public DateTime Endtime { get; set; }
        public required string FaceImage { get; set; }
        public required string FingerPrintImage {  get; set; }

    }
}
