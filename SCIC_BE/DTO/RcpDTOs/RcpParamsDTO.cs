using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Interfaces.IDto;

namespace SCIC_BE.DTO.RcpDTOs
{
    public class RcpParamsDTO : IRcpParams
    {
        public List<UserDTO> Users { get; set; }
        public List<Guid> DeviceId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
