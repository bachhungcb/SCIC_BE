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

        public AttendanceService(
            IAttendanceRepository attendanceRepository,
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

            // Group theo các trường đặc trưng của 1 buổi điểm danh
            var grouped = attendances.GroupBy(a => new
            {
                a.LecturerId,
                a.DeviceId,
                a.TimeStart,
                a.TimeEnd,
            });

            var result = new List<AttendanceDTO>();

            foreach (var group in grouped)
            {
                var lecturer = await _lecturerService.GetLecturerByIdAsync(group.Key.LecturerId);
                var studentDtos = new List<AttendanceStudentDTO>();

                foreach (var record in group)
                {
                    var student = await _studentService.GetStudentByIdAsync(record.StudentId);
                    if (student == null)
                        continue; // hoặc throw exception nếu muốn

                    studentDtos.Add(new AttendanceStudentDTO
                    {
                        Student = student,
                        IsAttended = record.IsAttended
                    });
                }

                // Loại bỏ sinh viên trùng
                studentDtos = studentDtos
                    .GroupBy(s => s.Student.UserId)
                    .Select(g => g.First())
                    .ToList();

                result.Add(new AttendanceDTO
                {
                    Id = group.First().Id,
                    Lecturer = lecturer,
                    Student = studentDtos,
                    DeviceId = group.Key.DeviceId,
                    TimeStart = group.Key.TimeStart,
                    TimeEnd = group.Key.TimeEnd,
                    CreatedAt = group.First().CreatedAt
                });
            }

            return result;
        }

        public async Task<AttendanceDTO> GetAttendanceByIdAsync(Guid id)
        {
            var attendances = await GetListAttendanceAsync();
            
            
            var attendance = attendances.FirstOrDefault(a => a.Id == id);
            if (attendance == null)
                throw new Exception("Attendance not found");
            
            var result = new AttendanceDTO();
            
            return attendance;
        }

        public async Task<CreateAttendanceRCPDto> CreateAttendanceAsync(CreateAttendanceDTO attendanceInfo)
        {
            var attendanceStudents = new List<AttendanceStudentDTO>();

            foreach (var studentId in attendanceInfo.StudentIds)
            {
                var student = await _studentService.GetStudentByIdAsync(studentId)
                    ?? throw new Exception($"Student with id {studentId} not found");

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

            var createAttendanceRcp = new CreateAttendanceRCPDto
            {
                LecturerId = attendanceInfo.LecturerId,
                AttendanceStudents = attendanceStudents,
                DeviceId = attendanceInfo.DeviceId,
                TimeStart = attendanceInfo.TimeStart,
                TimeEnd = attendanceInfo.TimeEnd,
                CreatedAt = DateTime.UtcNow,
            };

            var rpcRequestDto = new RcpRequestDTO
            {
                DeviceId = createAttendanceRcp.DeviceId,
                Method = "createPermission",
                Params = createAttendanceRcp
            };

            try
            {
                await _rcpService.SendRpcRequestAsync(rpcRequestDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing RPC request: {ex.Message}");
                throw new Exception("Error occurred while processing RPC request");
            }

            return createAttendanceRcp;
        }

        public async Task<List<AttendanceModel>> UpdateAttendanceAsync(Guid attendanceId, UpdateAttendanceDTO updateInfo)
        {
            var updatedAttendances = new List<AttendanceModel>();
            var attendanceStudents = new List<AttendanceStudentDTO>();

            foreach (var studentId in updateInfo.StudentIds)
            {
                var existingAttendance = await _attendanceRepository.GetByAttendanceIdAsync(attendanceId)
                    ?? throw new Exception("Attendance not found");
                
                var student = await _studentService.GetStudentByIdAsync(studentId)
                              ?? throw new Exception($"Student with id {studentId} not found");

                attendanceStudents.Add(new AttendanceStudentDTO
                {
                    Student = student,
                    IsAttended = false
                });

                
                existingAttendance.DeviceId = updateInfo.DeviceId;
                existingAttendance.StudentId = studentId;
                existingAttendance.LecturerId = updateInfo.LecturerId;
                
                
                await _attendanceRepository.UpdateAsync(existingAttendance);
                updatedAttendances.Add(existingAttendance);
            }
            var updateAttendanceRcp = new UpdateAttendanceRcpDto()
            {
                LecturerId = updateInfo.LecturerId,
                AttendanceStudents = attendanceStudents,
                DeviceId = updateInfo.DeviceId,
            };

            var rpcRequestDto = new RcpRequestDTO
            {
                DeviceId = updateAttendanceRcp.DeviceId,
                Method = "updatePermission",
                Params = updateAttendanceRcp
            };

            try
            {
                await _rcpService.SendRpcRequestAsync(rpcRequestDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing RPC request: {ex.Message}");
                throw new Exception("Error occurred while processing RPC request");
            }
          
            return updatedAttendances;
        }

        public async Task DeleteAttendanceAsync(Guid attendanceId)
        {
            var attendance = await _attendanceRepository.GetByAttendanceIdAsync(attendanceId)
                ?? throw new Exception("Attendance info not found");
            
            await _attendanceRepository.DeleteAsync(attendanceId);
        }
    }
}
