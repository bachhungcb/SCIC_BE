using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCIC_BE.DTO.StudentDTOs
{
    public class StudentDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } //From User
        public string Email { get; set; } //From User
        public string StudentCode { get; set; }
        public string FaceImage { get; set; }
        public DateTime EnrollDate { get; set; }

    }
}
