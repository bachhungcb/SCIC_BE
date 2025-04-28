using SCIC_BE.Models;

namespace SCIC_BE.Interfaces.IServices
{
    public interface IPasswordService
    {
        string HashPassword(UserModel user, string password);
        bool VerifyPassword(UserModel model, string hashedPassword, string providedPassword);
    }
}
