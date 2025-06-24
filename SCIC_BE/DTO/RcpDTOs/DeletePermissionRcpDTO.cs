using System;
using SCIC_BE.Interfaces.IDto;

namespace SCIC_BE.DTO.RcpDTOs;

public class DeletePermissionRcpDTO : IRcpParams
{
    public Guid permissionId { get; set; }
}