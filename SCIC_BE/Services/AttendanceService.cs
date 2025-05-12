using SCIC_BE.DTO.AttendanceDTOs;
using SCIC_BE.DTO.StudentDTOs;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.AttendanceRepository;

namespace SCIC_BE.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IStudentService _studentService;

        public AttendanceService(IAttendanceRepository attendanceRepository,
                                    IStudentService studentService)
        {
            _attendanceRepository = attendanceRepository;
            _studentService = studentService;
        }

        public async Task<List<AttendanceModel>> GetListAttendanceAsync()
        {
            var attendances = await _attendanceRepository.GetAllAttendanceAsync();

            return attendances ?? throw new Exception("Attendances not found");
        }

        public async Task<AttendanceModel> GetAttendanceByIdAsync(Guid id)
        {
            var attendance = await _attendanceRepository.GetByAttendanceIdAsync(id);

            return attendance ?? throw new Exception("Attendance not found");
        }

        public async Task CreateAttendanceAsync(CreateAttendanceDTO attendanceInfo)
        {
            var createAttendanceInfo = new AttendanceModel();

            var studentList = new List<AttendanceStudentDTO>();
            foreach (var studentId in attendanceInfo.StudentIds)
            {
                var student = await _studentService.GetStudentByIdAsync(studentId);

                var addStudent = new AttendanceStudentDTO()
                {
                    Student = student
                };
                studentList.Add(addStudent);
            }

            createAttendanceInfo.Id = Guid.NewGuid();
            createAttendanceInfo.Students = studentList;
            createAttendanceInfo.LecturerId = attendanceInfo.LecturerId;
            createAttendanceInfo.DeviceId = attendanceInfo.DeviceId;
            createAttendanceInfo.TimeEnd = attendanceInfo.TimeEnd;
            createAttendanceInfo.TimeStart = attendanceInfo.TimeStart;
            createAttendanceInfo.CreatedAt = DateTime.UtcNow;

            await _attendanceRepository.AddAsync(createAttendanceInfo);

        }

        public async Task UpdateAttendanceAsync(Guid id, UpdateAttendanceDTO updateInfo)
        {
            var attendance = await _attendanceRepository.GetByAttendanceIdAsync(id);

            var studentList = new List<AttendanceStudentDTO>();
            if (attendance == null)
                throw new Exception("attendance info not found");


            foreach (var studentId in updateInfo.StudentIds)
            {
                var student = await _studentService.GetStudentByIdAsync(studentId);

                var addStudent = new AttendanceStudentDTO()
                {
                    Student = student
                };
                studentList.Add(addStudent);
            }
            attendance.LecturerId = updateInfo.LecturerId;
            attendance.Students = studentList;
            attendance.DeviceId = updateInfo.DeviceId;

            await _attendanceRepository.UpdateAsync(attendance);
        }


    }
}
