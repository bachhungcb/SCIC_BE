using System;
using SCIC_BE.DTO.StudentDTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCIC_BE.Models
{
    public class AttendanceModel
    {
        public Guid Id { get; set; }
        public Guid LecturerId { get; set; }
        public Guid StudentId { get; set; }
        public bool IsAttended { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
