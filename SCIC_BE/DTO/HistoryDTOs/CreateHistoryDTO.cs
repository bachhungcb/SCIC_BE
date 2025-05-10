namespace SCIC_BE.DTO.HistoryDTOs
{
    public class CreateHistoryDTO
    {
        public Guid UserId { get; set; }
        public Guid DeviceId { get; set; }
        public string Type { get; set; } = string.Empty; //Loại: vào hay ra
        public string Status { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
