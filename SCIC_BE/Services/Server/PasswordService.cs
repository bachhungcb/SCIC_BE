using Microsoft.AspNetCore.Identity;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;

namespace SCIC_BE.Services.Server
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<UserModel> _passwordHasher = new PasswordHasher<UserModel>();

        public string HashPassword(UserModel user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(UserModel model, string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(model, hashedPassword, providedPassword);

            return result == PasswordVerificationResult.Success;
        }
    }
}
