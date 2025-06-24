using System;
using System.ComponentModel.DataAnnotations;

namespace SCIC_BE.Models
{
    public class StudentModel
    {
        [Key]
        public Guid UserId { get; set; } //Foreign key and Primary key
        public required string StudentCode { get; set; }
        public DateTime EnrollDate { get; set; }

        //Navigation
        public UserModel User { get; set; }
    }
}
