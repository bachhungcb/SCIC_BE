using SCIC_BE.Models;

namespace SCIC_BE.DTO.UserDTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string FullName { get; set; }
        public required string IdNumber { get; set; }
        public required string Email { get; set; }
        public string FaceImage { get; set; }
        public string FingerprintImage { get; set; }

        public ICollection<UserRoleDTO> UserRoles { get; set; }
    }
}
