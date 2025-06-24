using System.Collections.Generic;

namespace SCIC_BE.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        //Navigation
        public ICollection<UserRoleModel> UserRoles { get; set; }
    }
}
