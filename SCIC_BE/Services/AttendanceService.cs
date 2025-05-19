using SCIC_BE.DTO.AttendanceDTOs;
using SCIC_BE.DTO.RcpDTOs;
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
        private readonly RcpService _rcpService;

        public AttendanceService(IAttendanceRepository attendanceRepository,
                                    IStudentService studentService,
                                    ILecturerService lecturerService,
                                    RcpService rcpService)
        {
            _attendanceRepository = attendanceRepository;
            _studentService = studentService;
            _lecturerService = lecturerService;
            _rcpService = rcpService;
        }

        public async Task<List<AttendanceDTO>> GetListAttendanceAsync()
        {
            var attendances = await _attendanceRepository.GetAllAttendanceAsync();

            if (attendances == null || !attendances.Any())
                throw new Exception("Attendances not found");

            var groupedAttendances = attendances
                .GroupBy(a => new
                {
                    a.LecturerId,
                    a.DeviceId,
                    a.TimeStart,
                    a.TimeEnd,
                });

            var attendanceDtos = new List<AttendanceDTO>();

            foreach (var group in groupedAttendances)
            {
                var lecturer = await _lecturerService.GetLecturerByIdAsync(group.Key.LecturerId);
                var studentDtos = new List<AttendanceStudentDTO>();

                foreach (var record in group)
                {
                    var student = await _studentService.GetStudentByIdAsync(record.StudentId);
                    studentDtos.Add(new AttendanceStudentDTO
                    {
                        Student = student,
                        IsAttended = record.IsAttended // hoặc lấy từ DB nếu có
                    });
                }
                // Loại bỏ sinh viên trùng
                studentDtos = studentDtos
                    .GroupBy(s => s.Student.UserId)
                    .Select(g => g.First())
                    .ToList();

                attendanceDtos.Add(new AttendanceDTO
                {
                    Id = group.First().Id, // lấy ID đầu tiên từ group
                    Lecturer = lecturer,
                    Student = studentDtos,
                    DeviceId = group.Key.DeviceId,
                    TimeStart = group.Key.TimeStart,
                    TimeEnd = group.Key.TimeEnd,
                    CreatedAt = group.First().CreatedAt
                });
            }

            return attendanceDtos;
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

        public async Task<CreateAttendanceRCPDto> CreateAttendanceAsync(CreateAttendanceDTO attendanceInfo)
        {
            var attendanceStudents = new List<AttendanceStudentDTO>();

            foreach (var studentId in attendanceInfo.StudentIds)
            {
                var student = await _studentService.GetStudentByIdAsync(studentId);
                if (student == null)
                    throw new Exception($"Student with id {studentId} not found");

                attendanceStudents.Add(new AttendanceStudentDTO
                {
                    Student = student,
                    IsAttended = false
                });

                var attendance = new AttendanceModel
                {
                    Id = Guid.NewGuid(),
                    LecturerId = attendanceInfo.LecturerId,
                    StudentId = studentId,
                    DeviceId = attendanceInfo.DeviceId,
                    TimeStart = attendanceInfo.TimeStart,
                    TimeEnd = attendanceInfo.TimeEnd,
                    CreatedAt = DateTime.UtcNow
                };

                await _attendanceRepository.AddAsync(attendance);
            }

            var createAttendanceRcp = new CreateAttendanceRCPDto()
            {
                LecturerId = attendanceInfo.LecturerId,
                AttendanceStudents = attendanceStudents, // gán danh sách chi tiết
                DeviceId = attendanceInfo.DeviceId,
                TimeStart = attendanceInfo.TimeStart,
                TimeEnd = attendanceInfo.TimeEnd,
                CreatedAt = DateTime.UtcNow,
            };
            
            var rpcRequestDto = new RcpRequestDTO()
            {
                DeviceId = createAttendanceRcp.DeviceId,
                Method = "createPermission",
                Params = createAttendanceRcp
            };
            try
            {
                // Gửi yêu cầu RPC
                await _rcpService.SendRpcRequestAsync(rpcRequestDto);
                
            }
            catch (Exception ex)
            {
                // Log thông tin chi tiết về lỗi
                Console.WriteLine($"Error occurred while processing RPC request for : {ex.Message}");
                throw new Exception($"Error occurred while processing RPC request for");
            }
            
            return createAttendanceRcp;
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
        public async Task DeleteAttendanceAsync(Guid AttendanceId)
        {
            var attendance = await _attendanceRepository.GetByAttendanceIdAsync(AttendanceId);

            if (attendance == null)
                throw new Exception("attendance info not found");
            await _attendanceRepository.DeleteAsync(AttendanceId);
        }

    }
}
