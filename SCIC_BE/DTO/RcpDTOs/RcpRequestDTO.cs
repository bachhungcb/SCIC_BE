using System;
using SCIC_BE.Interfaces.IDto;

namespace SCIC_BE.DTO.RcpDTOs
{
    public class RcpRequestDTO 
    {
        public required string Method {  get; set; }
        public Guid DeviceId { get; set; }
        public IRcpParams Params { get; set; }
    }
}
