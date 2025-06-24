using System;
using System.Collections.Generic;

namespace SCIC_BE.DTO.AttendanceDTOs
{
    public class UpdateAttendanceDTO
    {
        public Guid LecturerId { get; set; }
        public required List<Guid> StudentIds { get; set; }
        public Guid DeviceId { get; set; }
    }
}
