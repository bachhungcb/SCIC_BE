using DocumentFormat.OpenXml.Bibliography;

namespace SCIC_BE.DTO.StudentDTOs
{
    public class AttendanceStudentDTO
    {
        public required StudentDTO Student { get; set; }
        public bool IsAttended { get; set; } = false;
    }
}
