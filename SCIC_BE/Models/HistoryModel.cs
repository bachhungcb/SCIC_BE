using System.ComponentModel.DataAnnotations;

namespace SCIC_BE.Models
{
    public class HistoryModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId {  get; set; }
        public Guid DeviceId { get; set; }
        public string Type { get; set; } = string.Empty; //Loại: vào hay ra
        public string Status { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }

    }
}
