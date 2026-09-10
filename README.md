# Secure Password Generator

A console-based Secure Password Generator application developed using C# and .NET.

The application allows users to generate secure and customizable passwords based on their preferences. Generated passwords and their strength levels can also be saved and viewed using SQL Server.

## Features

- Generate secure random passwords
- Choose password length
- Include uppercase letters
- Include lowercase letters
- Include numbers
- Include special characters
- Password strength checking
- Generate multiple passwords
- Password history
- Save generated passwords to SQL Server
- View saved password history
- Store user names with passwords

## Technologies Used

- C#
- .NET
- SQL Server
- Microsoft.Data.SqlClient
- Git
- GitHub
- Visual Studio

## How It Works

1. Run the application.
2. Select **Generate Password** from the menu.
3. Enter your name.
4. Choose the password length.
5. Select the character types to include.
6. The application generates a secure password.
7. The password strength is calculated.
8. The password and strength are saved to SQL Server.
9. Select **View Password History** to see previously generated passwords.

## Password Strength

The application checks password strength based on factors such as:

- Password length
- Uppercase letters
- Lowercase letters
- Numbers
- Special characters

Passwords are categorized as:

- Weak
- Medium
- Strong

## Database

The application uses SQL Server to store password history.

The password history contains:

- ID
- User Name
- Generated Password
- Password Strength
- Created Date

## Application Screenshots

### Main Menu

![Main Menu](screenshots/main-menu.png/Screenshot%202026-09-10%20135003.png)

### Generate Password

![Generate Password](screenshots/generate-password.png/Screenshot%202026-09-10%20134811.png)

### Password History

![Password History](screenshots/password-history.png/Screenshot%202026-09-10%20134836.png)

## Author

W.A.D. Raveesha Kavindi

## Project Status

Completed
