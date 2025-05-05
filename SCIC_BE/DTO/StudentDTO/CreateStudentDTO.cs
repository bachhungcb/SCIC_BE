namespace SCIC_BE.DTO.StudentDTO
{
    public class CreateStudentDTO
    {
        public Guid UserId { get; set; }
        public required string StudentCode { get; set; }
        public DateTime EnrollDate { get; set; }
    }
}
