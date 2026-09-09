using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;

namespace SecurePasswordGenerator
{
    internal class Program
    {
        // Temporary password history while program is running
        static List<string> passwordHistory = new List<string>();

        // SQL Server connection
        static string connectionString =
            "Server=localhost;Database=PasswordGeneratorDB;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.Clear();

                Console.WriteLine("================================");
                Console.WriteLine("   SECURE PASSWORD GENERATOR");
                Console.WriteLine("================================");
                Console.WriteLine();

                Console.WriteLine("1. Generate Password");
                Console.WriteLine("2. View Password History");
                Console.WriteLine("3. Exit");

                Console.WriteLine();
                Console.Write("Enter your choice (1-3): ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        GeneratePasswordMenu();
                        break;

                    case "2":
                        ViewPasswordHistory();
                        break;

                    case "3":
                        running = false;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                        Pause();
                        break;
                }
            }

            Console.Clear();
            Console.WriteLine("Thank you for using Secure Password Generator!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void GeneratePasswordMenu()
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("       GENERATE PASSWORD");
            Console.WriteLine("================================");
            Console.WriteLine();

            // Ask for name
            Console.Write("Enter your name: ");
            string? userName = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(userName))
            {
                Console.Write("Name cannot be empty. Enter your name: ");
                userName = Console.ReadLine();
            }

            // Ask for password length
            Console.WriteLine();
            Console.Write("Enter password length (8-50): ");

            int length;

            while (!int.TryParse(Console.ReadLine(), out length)
                   || length < 8
                   || length > 50)
            {
                Console.Write("Invalid input. Enter a number between 8 and 50: ");
            }

            // Password options
            bool includeUppercase =
                GetYesNoAnswer("Include uppercase letters? (Y/N): ");

            bool includeLowercase =
                GetYesNoAnswer("Include lowercase letters? (Y/N): ");

            bool includeNumbers =
                GetYesNoAnswer("Include numbers? (Y/N): ");

            bool includeSymbols =
                GetYesNoAnswer("Include special characters? (Y/N): ");

            // At least one option must be selected
            while (!includeUppercase &&
                   !includeLowercase &&
                   !includeNumbers &&
                   !includeSymbols)
            {
                Console.WriteLine();
                Console.WriteLine("You must select at least one character type.");

                includeUppercase =
                    GetYesNoAnswer("Include uppercase letters? (Y/N): ");

                includeLowercase =
                    GetYesNoAnswer("Include lowercase letters? (Y/N): ");

                includeNumbers =
                    GetYesNoAnswer("Include numbers? (Y/N): ");

                includeSymbols =
                    GetYesNoAnswer("Include special characters? (Y/N): ");
            }

            // Generate password
            string password = CreatePassword(
                length,
                includeUppercase,
                includeLowercase,
                includeNumbers,
                includeSymbols);

            // Check password strength
            string strength = CheckStrength(password);

            // Save temporarily in program history
            passwordHistory.Add(
                $"Name: {userName} | Password: {password} | Strength: {strength}");

            // Save permanently in SQL Server
            SavePasswordToDatabase(userName, password, strength);

            Console.WriteLine();
            Console.WriteLine("Generated Password: " + password);
            Console.WriteLine("Password Strength: " + strength);

            Pause();
        }

        static void ViewPasswordHistory()
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("       PASSWORD HISTORY");
            Console.WriteLine("================================");
            Console.WriteLine();

            if (passwordHistory.Count == 0)
            {
                Console.WriteLine("No passwords generated yet.");
            }
            else
            {
                for (int i = 0; i < passwordHistory.Count; i++)
                {
                    Console.WriteLine(
                        $"{i + 1}. {passwordHistory[i]}");
                }
            }

            Pause();
        }

        static bool GetYesNoAnswer(string question)
        {
            while (true)
            {
                Console.Write(question);

                string? answer =
                    Console.ReadLine()?.Trim().ToUpper();

                if (answer == "Y" || answer == "YES")
                {
                    return true;
                }

                if (answer == "N" || answer == "NO")
                {
                    return false;
                }

                Console.WriteLine(
                    "Invalid input. Please enter Y or N.");
            }
        }

        static string CreatePassword(
            int length,
            bool includeUppercase,
            bool includeLowercase,
            bool includeNumbers,
            bool includeSymbols)
        {
            string characters = "";

            if (includeUppercase)
            {
                characters += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }

            if (includeLowercase)
            {
                characters += "abcdefghijklmnopqrstuvwxyz";
            }

            if (includeNumbers)
            {
                characters += "0123456789";
            }

            if (includeSymbols)
            {
                characters += "!@#$%^&*";
            }

            char[] password = new char[length];

            for (int i = 0; i < length; i++)
            {
                int randomIndex =
                    RandomNumberGenerator.GetInt32(
                        characters.Length);

                password[i] =
                    characters[randomIndex];
            }

            return new string(password);
        }

        static string CheckStrength(string password)
        {
            int score = 0;

            if (password.Length >= 8)
            {
                score++;
            }

            if (password.Any(char.IsUpper))
            {
                score++;
            }

            if (password.Any(char.IsLower))
            {
                score++;
            }

            if (password.Any(char.IsDigit))
            {
                score++;
            }

            if (password.Any(
                character => !char.IsLetterOrDigit(character)))
            {
                score++;
            }

            if (score <= 2)
            {
                return "Weak";
            }

            if (score <= 4)
            {
                return "Medium";
            }

            return "Strong";
        }

        static void SavePasswordToDatabase(
            string userName,
            string password,
            string strength)
        {
            try
            {
                using SqlConnection connection =
                    new SqlConnection(connectionString);

                connection.Open();

                string query =
                    @"INSERT INTO PasswordHistory
                      (UserName, GeneratedPassword, Strength)
                      VALUES
                      (@UserName, @Password, @Strength)";

                using SqlCommand command =
                    new SqlCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@UserName", userName);

                command.Parameters.AddWithValue(
                    "@Password", password);

                command.Parameters.AddWithValue(
                    "@Strength", strength);

                command.ExecuteNonQuery();

                Console.WriteLine();
                Console.WriteLine(
                    "Password saved to database successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Database error: " + ex.Message);
            }
        }

        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine(
                "Press any key to return to the menu...");

            Console.ReadKey();
        }
    }
}