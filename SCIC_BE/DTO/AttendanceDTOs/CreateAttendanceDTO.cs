using System;
using System.Collections.Generic;
using SCIC_BE.DTO.StudentDTOs;

namespace SCIC_BE.DTO.AttendanceDTOs
{
    public class CreateAttendanceDTO
    {
        public Guid LecturerId { get; set; }
        public required List<Guid> StudentIds { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
    }
}
