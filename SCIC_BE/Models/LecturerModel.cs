using System;
using System.ComponentModel.DataAnnotations;

namespace SCIC_BE.Models
{
    public class LecturerModel
    {
        [Key]
        public Guid UserId { get; set; }

        public required string LecturerCode {  get; set; }
        public DateTime HireDate { get; set; }

        //Navigation
        public UserModel? User { get; set; }
    }
}
