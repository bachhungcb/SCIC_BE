using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.Models;

namespace SCIC_BE.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ScicDbContext _context;

        public UserRepository(ScicDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users
                        .Include(u => u.StudentInfo)
                        .Include(u => u.UserRoles)
                        .Include(u => u.LecturerInfo)
                        .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task AddUserAsync(UserModel user)
        {
            // Thêm người dùng vào cơ sở dữ liệu
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();  // Lưu thay đổi vào cơ sở dữ liệu
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var users = await _context.Users
                        .Include(u => u.StudentInfo)
                        .Include(u => u.UserRoles)
                        .Include(u => u.LecturerInfo)
                        .ToListAsync();


            if (users == null)
            {
                return null;
            }

            return users;
        }
    }
}
