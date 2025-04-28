namespace SCIC_BE.DTO.StudentDTO
{
    public class UpdateStudentDTO
    {
        public Guid UserId { get; set; }
        public required string NewStudentCode { get; set; }
    }
}
