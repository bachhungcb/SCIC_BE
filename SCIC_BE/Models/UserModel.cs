namespace SCIC_BE.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }

        //Navigation
        public ICollection<UserRoleModel> UserRoles { get; set; }
        public StudentInfoModel StudentInfo { get; set; }
        public LecturerInfoModel LecturerInfo { get; set; }

    }
}
