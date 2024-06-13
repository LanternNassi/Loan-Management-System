using Microsoft.AspNetCore.Identity;

namespace Loan_Management_System.Utils
{
    public static class PasswordHasherUtility
    {
        private static readonly PasswordHasher<object> passwordHasher = new PasswordHasher<object>();

        public static string HashPassword(string password)
        {
            return passwordHasher.HashPassword(null, password);
        }

        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
