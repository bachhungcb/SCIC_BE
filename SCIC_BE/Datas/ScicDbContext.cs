using Microsoft.EntityFrameworkCore;
using SCIC_BE.Models;

namespace SCIC_BE.Data
{
    public class ScicDbContext : DbContext
    {
        public ScicDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<StudentInfoModel> StudentInfos { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<LecturerInfoModel> LecturerInfos { get; set; }
        public DbSet<UserRoleModel> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
            modelBuilder.Entity<StudentInfoModel>()
                .HasOne(s => s.User)
                .WithOne(u => u.StudentInfo)
                .HasForeignKey<StudentInfoModel>(s => s.UserId);

            // Config bảng LecturerInfo 1-1 với User
            modelBuilder.Entity<LecturerInfoModel>()
                .HasOne(l => l.User)
                .WithOne(u => u.LecturerInfo)
                .HasForeignKey<LecturerInfoModel>(l => l.UserId);
        }

    }
}
