using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.Models;
using System.Runtime.InteropServices;

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
                        .ThenInclude(ur => ur.Role)
                        .Include(u => u.LecturerInfo)
                        .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
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
                        .ThenInclude(ur => ur.Role)
                        .Include(u => u.LecturerInfo)
                        .ToListAsync();


            if (users == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return users;
        }

        public async Task<UserModel> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                        .Include(u => u.StudentInfo)
                        .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                        .Include(u => u.LecturerInfo)
                        .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task UpdateUserAsync(UserModel user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found to update");
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found to update");
            }

            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();
        }

    }
}
