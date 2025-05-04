namespace SCIC_BE.Models
{
    public class ApiError
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string? ErrorCode { get; set; }
        public string TraceId { get; set; }
        public string Timestamp { get; set; }
        public string? Details { get; set; }
    }

}
