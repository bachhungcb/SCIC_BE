using System.ComponentModel.DataAnnotations;

namespace SCIC_BE.DTO.PermissionDataRequestDTOs
{
    public class PermissionDTO
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
