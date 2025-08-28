using SCIC_BE.DTO.StudentDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace SCIC_BE.Models
{
    public class AttendanceModel
    {
        public Guid Id { get; set; }
        public Guid LecturerId { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime CreatedAt { get; set; }

        // Đây là cột JSON trong database
        public string AttendanceData { get; set; }

        // Không mapping vào DB, dùng để code thuận tiện
        [NotMapped]
        public List<AttendanceStudent> Students
        {
            get => string.IsNullOrEmpty(AttendanceData)
                ? new List<AttendanceStudent>()
                : JsonSerializer.Deserialize<List<AttendanceStudent>>(AttendanceData);

            set => AttendanceData = JsonSerializer.Serialize(value);
        }
    }

    public class AttendanceStudent
    {
        public Guid StudentId { get; set; }
        public bool IsAttended { get; set; }
    }
}
