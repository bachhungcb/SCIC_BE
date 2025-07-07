using System;

namespace SCIC_BE.DTO.LecturerDTOs
{
    public class LecturerDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } //From User
        public string Email { get; set; } //From User
        public string LecturerCode { get; set; }
        public string? FaceImage { get; set; }
        public string? FingerprintImage { get; set; }
        public DateTime HireDate { get; set; }
    }
}
