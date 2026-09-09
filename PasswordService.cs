using System;
using System.Linq;
using System.Security.Cryptography;

namespace SecurePasswordGenerator
{
    public class PasswordService
    {
        public string GeneratePassword(int length)
        {
            const string characters =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "abcdefghijklmnopqrstuvwxyz" +
                "0123456789" +
                "!@#$%^&*";

            char[] password = new char[length];

            for (int i = 0; i < length; i++)
            {
                int randomIndex =
                    RandomNumberGenerator.GetInt32(characters.Length);

                password[i] = characters[randomIndex];
            }

            return new string(password);
        }

        public string CheckStrength(string password)
        {
            int score = 0;

            if (password.Length >= 8)
                score++;

            if (password.Any(char.IsUpper))
                score++;

            if (password.Any(char.IsLower))
                score++;

            if (password.Any(char.IsDigit))
                score++;

            if (password.Any(ch => !char.IsLetterOrDigit(ch)))
                score++;

            if (score <= 2)
                return "Weak";
            else if (score <= 4)
                return "Medium";
            else
                return "Strong";
        }
    }
}