namespace SCIC_BE.DTO.UserDTOs
{
    public class CreateUserDTO
    {
        public required string UserName { get; set; }
        public required string FullName { get; set; }
        public required string IdNumber { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FaceImage { get; set; }
        public required string FingerprintImage { get; set; }
    }
}
