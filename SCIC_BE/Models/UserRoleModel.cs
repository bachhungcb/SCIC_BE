namespace SCIC_BE.Models
{
    public class UserRoleModel
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }

        //Navigation
        public required UserModel User { get; set; }
        public required RoleModel Role { get; set; }
    }
}
