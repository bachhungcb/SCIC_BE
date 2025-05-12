using System.ComponentModel.DataAnnotations.Schema;

namespace SCIC_BE.DTO.StudentDTOs
{
    [NotMapped]
    public class StudentDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } //From User
        public string Email { get; set; } //From User
        public string StudentCode { get; set; }
        public DateTime EnrollDate { get; set; }

    }
}
