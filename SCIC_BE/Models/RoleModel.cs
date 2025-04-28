namespace SCIC_BE.Models
{
    public class RoleModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        //Navigation
        public ICollection<UserRoleModel> UserRoles { get; set; }
    }
}
