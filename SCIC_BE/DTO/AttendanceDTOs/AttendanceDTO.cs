using System;
using System.Collections.Generic;
using SCIC_BE.DTO.LecturerDTOs;
using SCIC_BE.DTO.StudentDTOs;

namespace SCIC_BE.DTO.AttendanceDTOs;

public class AttendanceDTO
{
    public Guid Id { get; set; }
    public LecturerDTO Lecturer { get; set; }
    public List<AttendanceStudentDTO> Student { get; set; }
    public Guid DeviceId { get; set; }
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public DateTime CreatedAt { get; set; }
}