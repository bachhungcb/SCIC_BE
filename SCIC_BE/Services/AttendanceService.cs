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
        private readonly ILecturerService _lecturerService;

        public AttendanceService(IAttendanceRepository attendanceRepository,
                                    IStudentService studentService,
                                    ILecturerService lecturerService)
        {
            _attendanceRepository = attendanceRepository;
            _studentService = studentService;
            _lecturerService = lecturerService;
        }

        public async Task<List<AttendanceDTO>> GetListAttendanceAsync()
        {
            var attendances = await _attendanceRepository.GetAllAttendanceAsync();
            
            if (attendances == null || !attendances.Any())
                throw new Exception("Attendances not found");

            // Gom nhóm theo các trường đặc trưng 1 buổi điểm danh
            var groupedAttendances = attendances
                .GroupBy(a => new { a.Id, a.LecturerId, a.DeviceId, a.TimeStart, a.TimeEnd, a.CreatedAt });

            var attendanceDtOs = new List<AttendanceDTO>();

            foreach (var group in groupedAttendances)
            {
                // Lấy lecturer
                var lecturer = await _lecturerService.GetLecturerByIdAsync(group.Key.LecturerId);

                // Lấy danh sách sinh viên cho buổi điểm danh này
                var studentDtOs = new List<AttendanceStudentDTO>();
                foreach (var attendance in group)
                {
                    try
                    {
                        var student = await _studentService.GetStudentByIdAsync(attendance.StudentId);
                        var attendStudent = new AttendanceStudentDTO()
                        {
                            Student = student,
                            IsAttended = false
                        };
                        
                        studentDtOs.Add(attendStudent);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }

                attendanceDtOs.Add(new AttendanceDTO
                {
                    Id = group.Key.Id,
                    Lecturer = lecturer,
                    Student = studentDtOs,
                    DeviceId = group.Key.DeviceId,
                    TimeStart = group.Key.TimeStart,
                    TimeEnd = group.Key.TimeEnd,
                    CreatedAt = group.Key.CreatedAt
                });
            }

            return attendanceDtOs;
        }



        public async Task<AttendanceDTO> GetAttendanceByIdAsync(Guid id)
        {
            var attendance = await _attendanceRepository.GetByAttendanceIdAsync(id);
            var student = await _studentService.GetStudentByIdAsync(attendance.StudentId);
            var lecturer = await _lecturerService.GetLecturerByIdAsync(attendance.LecturerId);
            var studentList = new List<AttendanceStudentDTO>();
            var attendStudent = new AttendanceStudentDTO()
            {
                Student = student,
                IsAttended = false
            };
            
            studentList.Add(attendStudent);
            
            var attendanceDto = new AttendanceDTO()
            {
                Id = attendance.Id,
                Lecturer = lecturer,
                Student = studentList,
                DeviceId = attendance.DeviceId,
                TimeStart = attendance.TimeStart,
                TimeEnd = attendance.TimeEnd,
                CreatedAt = attendance.CreatedAt
            };
            
            return attendanceDto ?? throw new Exception("Attendance not found");
        }

        public async Task<List<AttendanceModel>> CreateAttendanceAsync(CreateAttendanceDTO attendanceInfo)
        {
            var createdAttendanceInfo = new List<AttendanceModel>();
            foreach (var studentId in attendanceInfo.StudentIds)
            {
                var attendance = new AttendanceModel
                {
                    Id = Guid.NewGuid(),
                    DeviceId = attendanceInfo.DeviceId,
                    LecturerId = attendanceInfo.LecturerId,
                    StudentId = studentId,
                    TimeStart = attendanceInfo.TimeStart,
                    TimeEnd = attendanceInfo.TimeEnd,
                    CreatedAt = DateTime.UtcNow,
                };
                
                await _attendanceRepository.AddAsync(attendance);
                createdAttendanceInfo.Add(attendance);
            }
            
            //TODO: implement RCP request for create Attendance
            
            return createdAttendanceInfo;
        }


        public async Task<List<AttendanceModel>> UpdateAttendanceAsync(Guid attendanceId, UpdateAttendanceDTO updateInfo)
        {
            
           var updatedAttendanceInfo = new List<AttendanceModel>();
           foreach (var studentId in updateInfo.StudentIds)
           {
               var existingAttendance = await _attendanceRepository.GetByAttendanceIdAsync(attendanceId);

               if (existingAttendance == null)
               {
                   throw new Exception("Attendance not found");
               }
               existingAttendance.DeviceId = updateInfo.DeviceId;
               existingAttendance.StudentId = studentId;
               existingAttendance.LecturerId = updateInfo.LecturerId;
               await _attendanceRepository.UpdateAsync(existingAttendance);
               updatedAttendanceInfo.Add(existingAttendance);
           }
            
           return updatedAttendanceInfo;  
        }
        public async Task DeleteAttendanceAsync(Guid id)
        {
            var attendance = await _attendanceRepository.GetByAttendanceIdAsync(id);

            if (attendance == null)
                throw new Exception("attendance info not found");
            await _attendanceRepository.DeleteAsync(id);
        }

    }
}
