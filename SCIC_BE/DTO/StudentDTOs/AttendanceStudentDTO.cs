using DocumentFormat.OpenXml.Bibliography;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCIC_BE.DTO.StudentDTOs
{
    [NotMapped]
    public class AttendanceStudentDTO
    {
        public required StudentDTO Student { get; set; }
        public bool IsAttended { get; set; } = false;
    }
}
