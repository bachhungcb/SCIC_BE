using System;
using System.Collections.Generic;
using SCIC_BE.DTO.StudentDTOs;
using SCIC_BE.Interfaces.IDto;

namespace SCIC_BE.DTO.RcpDTOs;

public class UpdateAttendanceRcpDto : IRcpParams
{
    public Guid LecturerId { get; set; }
    public Guid DeviceId { get; set; }
    public List<AttendanceStudentDTO>? AttendanceStudents { get; set; } // thêm danh sách sinh viên chi tiết
}