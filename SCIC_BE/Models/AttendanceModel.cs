namespace SCIC_BE.Models
{
    public class AttendanceModel
    {
        public Guid Id { get; set; }
        public Guid LecturerId { get; set; }
        public List<Guid> StudentIds { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
