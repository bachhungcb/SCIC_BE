using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SCIC_BE.DTO.UserDTOs;

namespace SCIC_BE.DTO.PermissionDataRequestDTOs
{
    public class PermissionDTO
    {
        [Key]
        public Guid Id { get; set; }
        public List<UserDTO> Users { get; set; }
        public List<Guid> DeviceIds { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
