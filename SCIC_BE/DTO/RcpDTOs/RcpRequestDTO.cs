namespace SCIC_BE.DTO.RcpDTOs
{
    public class RcpRequestDTO
    {
        public required string Token { get; set; } //JWT token
        public required string Method {  get; set; }
        public Guid DeviceId { get; set; }
        public RcpParamsDTO Params { get; set; }
    }
}
