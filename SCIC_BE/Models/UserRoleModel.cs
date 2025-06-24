using System;

namespace SCIC_BE.Models
{
    public class UserRoleModel
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }

        //Navigation
        public UserModel User { get; set; }
        public RoleModel Role { get; set; }
    }
}
