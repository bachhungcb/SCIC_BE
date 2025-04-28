using SCIC_BE.DTO.StudentDTO;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.UserRepository;
using SCIC_BE.Repository.StudentRepository;

namespace SCIC_BE.Services
{
    public class UserInfoService
    {
        private readonly IStudentInfoRepository _studentInfoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStudentService _studentService;
        public UserInfoService(IStudentInfoRepository studentInfoRepository, IUserRepository userRepository, IStudentService studentService)
        {
            _studentInfoRepository = studentInfoRepository;
            _userRepository = userRepository;
            _studentService = studentService;
        }

        public async Task CreateStudentAsync(CreateStudentDTO dto)
        {
            // Kiểm tra xem User đã tồn tại hay chưa
            var user = await _userRepository.GetUserByIdAsync(dto.UserId);

            if (user == null)
            {
                // Nếu User không tồn tại, tạo mới một User
                user = new UserModel
                {
                    Id = dto.UserId,
                    Name = dto.Name,
                    Email = dto.Email,
                    PasswordHash = "Testpwd"
                    // Thêm các thuộc tính khác của User nếu cần
                };
                await _userRepository.AddUserAsync(user);  // Giả sử có method này trong repository
            }

            // Tạo thông tin sinh viên (StudentInfo) sau khi chắc chắn User tồn tại
            var studentInfo = new StudentInfoModel
            {
                UserId = dto.UserId,
                StudentCode = dto.StudentCode,
                EnrollDate = dto.EnrollDate
            };

            // Lưu thông tin sinh viên vào cơ sở dữ liệu
            await _studentService.CreateStudentInfoAsync(dto.UserId, dto.StudentCode, dto.EnrollDate);

        }
        public async Task<UserModel> GetUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            return user;
        }

    }
}
