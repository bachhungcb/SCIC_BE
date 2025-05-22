using System.ComponentModel.DataAnnotations.Schema;
using SCIC_BE.DTO.RoleDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.DTO.UserDTOs
{
    [NotMapped]
    public class UserRoleDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

    }
}
