using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SCIC_BE.DTO.UserDTOs;

namespace SCIC_BE.Models
{
    public class PermissionModel
    {
        [Key]
        public Guid Id { get; set; }
        public ICollection<PermissionUser> PermissionUsers { get; set; }
        public List<Guid> DeviceId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
