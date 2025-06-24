using System;
using System.Collections.Generic;

namespace SCIC_BE.DTO.PermissionDataRequestDTOs
{
    public class PermissionDataRequestDTO
    {
        public List<Guid> UserIds { get; set; }
        public List<Guid> DeviceIds { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        
    }
}
