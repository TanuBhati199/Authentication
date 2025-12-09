using BCrypt.Net;

namespace CaseManagementSystem.Helpers
{
    public static class SecurityHelper
    {
        public static string Hash(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public static bool Verify(string input, string hashedInput)
        {
            return BCrypt.Net.BCrypt.Verify(input, hashedInput);
        }
    }

}
