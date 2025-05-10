namespace SCIC_BE.DTO.PermissionDataRequestDTOs
{
    public class PermissionDataRequestDTO
    {
        public List<string> UserIds { get; set; }
        public List<string> DeviceIds { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }

        public required string Token { get; set; }
    }
}
