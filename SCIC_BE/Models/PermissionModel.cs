using System.ComponentModel.DataAnnotations;

namespace SCIC_BE.Models
{
    public class PermissionModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DeviceId { get; set; }
        public required string UserName { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string FaceImage {  get; set; }
    }
}
