using System;

namespace SCIC_BE.Models;

public class AttendanceLogModel
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid DeviceId { get; set; }
    public string Status {get; set;}
    
    public DateTime CreatedOn { get; set; }
    
}