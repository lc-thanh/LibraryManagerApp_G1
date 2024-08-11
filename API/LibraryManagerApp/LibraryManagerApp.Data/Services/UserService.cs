using LibraryManagerApp.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagerApp.Data.Services
{
    public class UserService
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService()
        {
            _passwordHasher = new PasswordHasher<User>();
        }

        public string HashPassword(string password)
        {
            var user = new User();
            string hashedPassword = _passwordHasher.HashPassword(user, password);
            return hashedPassword;
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var user = new User();
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);

            return verificationResult == PasswordVerificationResult.Success;
        }
    }
}
