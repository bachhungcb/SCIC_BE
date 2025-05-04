namespace SCIC_BE.Models
{
    public class ApiErrorResult
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string TraceId { get; set; }
        public string Timestamp { get; set; }
    }

}
