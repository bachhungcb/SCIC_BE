using System;

namespace SCIC_BE.Models;

public class PermissionUser
{
    public Guid PermissionId { get; set; }
    public PermissionModel Permission { get; set; }

    public Guid UserId { get; set; }
    public UserModel User { get; set; }
}
