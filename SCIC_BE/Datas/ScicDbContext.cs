using Microsoft.EntityFrameworkCore;
using SCIC_BE.DTO.StudentDTOs;
using SCIC_BE.DTO.UserDTOs;
using SCIC_BE.Models;

namespace SCIC_BE.Data
{
    public class ScicDbContext : DbContext
    {
        public ScicDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<StudentModel> Student { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<LecturerModel> Lecturer { get; set; }
        public DbSet<UserRoleModel> UserRoles { get; set; }
        public DbSet<DeviceModel> Devices { get; set; }
        public DbSet<PermissionModel> Permissions { get; set; }
        public DbSet<AttendanceModel> Attendances { get; set; }
        public DbSet<AttendanceLogModel> AttendanceLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PermissionUser>()
                .HasKey(pu => new { pu.PermissionId, pu.UserId });

            // Config bảng UserRole: composite key
            modelBuilder.Entity<UserRoleModel>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRoleModel>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRoleModel>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Config bảng StudentInfo 1-1 với User
            modelBuilder.Entity<StudentModel>()
                .HasOne(s => s.User)
                .WithOne(u => u.StudentInfo)
                .HasForeignKey<StudentModel>(s => s.UserId);

            // Config bảng LecturerInfo 1-1 với User
            modelBuilder.Entity<LecturerModel>()
                .HasOne(l => l.User)
                .WithOne(u => u.LecturerInfo)
                .HasForeignKey<LecturerModel>(l => l.UserId);
        }

    }
}
