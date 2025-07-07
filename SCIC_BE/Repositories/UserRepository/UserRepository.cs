using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.Data;
using SCIC_BE.Models;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SCIC_BE.DTO.UserDTOs;

namespace SCIC_BE.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ScicDbContext _context;

        public UserRepository(ScicDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    IdNumber = u.IdNumber,
                    Email = u.Email,
                    FaceImage = u.FaceImage, // ,null
                    FingerprintImage = u.FingerprintImage, //null,
                    UserRoles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
                })
                .FirstOrDefaultAsync();

            // if (user == null)
            //     throw new KeyNotFoundException("User not found");

            return user;
        }

        public async Task AddUserAsync(UserModel user)
        {
            // Thêm người dùng vào cơ sở dữ liệu
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();  // Lưu thay đổi vào cơ sở dữ liệu
        }
        
        public async Task<UserModel> GetUserEntityByIdAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            // if (user == null)
            //     throw new KeyNotFoundException("User not found");

            return user;
        }

        
        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            int page = 1; int pageSize = 20;
            var skip = (page - 1) * pageSize;

            var userList = await _context.Users
                .OrderBy(u => u.Id)
                .Skip(skip)
                .Take(pageSize)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    IdNumber = u.IdNumber,
                    Email = u.Email,

                    //KHÔNG lấy ảnh ở đây để giảm tải JSON
                    FaceImage = null,
                    FingerprintImage = null,

                    UserRoles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
                })
                .ToListAsync();

            return userList;
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
