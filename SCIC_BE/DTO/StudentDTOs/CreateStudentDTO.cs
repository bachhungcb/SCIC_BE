using System;

namespace SCIC_BE.DTO.StudentDTOs
{
    public class CreateStudentDTO
    {
        public required string StudentCode { get; set; }
        public DateTime EnrollDate { get; set; }
    }
}
