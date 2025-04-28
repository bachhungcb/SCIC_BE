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
        public StudentModel StudentInfo { get; set; }
        public LecturerModel LecturerInfo { get; set; }

    }
}
