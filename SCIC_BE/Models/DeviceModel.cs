using System;

namespace SCIC_BE.Models
{
    public class DeviceModel
    {
        public Guid Id { get; set; }
        public required string MacAddress { get; set; }
        public string? Label { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }

    }
}
