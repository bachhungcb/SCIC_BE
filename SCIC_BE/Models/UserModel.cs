using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SCIC_BE.Models
{
    [Index(nameof(IdNumber), IsUnique =true)]
    public class UserModel
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string FullName { get; set; }
        public required string IdNumber { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string FaceImage {  get; set; }
        public string FingerprintImage {  get; set; }

        //Navigation
        public ICollection<UserRoleModel> UserRoles { get; set; }
        public StudentModel StudentInfo { get; set; }
        public LecturerModel LecturerInfo { get; set; }
        public ICollection<PermissionUser> PermissionUsers { get; set; }

    }
}
