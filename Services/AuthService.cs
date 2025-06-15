using BCrypt.Net;
using static System.Net.Mime.MediaTypeNames;
using System.IO.Packaging;

namespace WpfEfAuthen.Services
{
    public class AuthService
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }

}
