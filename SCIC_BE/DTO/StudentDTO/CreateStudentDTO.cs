namespace SCIC_BE.DTO.StudentDTO
{
    public class CreateStudentDTO
    {
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string StudentCode { get; set; }
        public DateTime EnrollDate { get; set; }
    }
}
