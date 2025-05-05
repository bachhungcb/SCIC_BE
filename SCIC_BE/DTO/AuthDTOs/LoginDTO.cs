using System.ComponentModel.DataAnnotations;

namespace SCIC_BE.DTO.AuthDTOs
{
    public class LoginDTO
    {
        public required string Email { get; set; } 
        public  required string Password { get; set; }
    }
}
