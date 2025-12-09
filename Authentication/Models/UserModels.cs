using System;

namespace CaseManagementSystem.Models
{
    // Base model for login/signup
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // Full user table
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // store hashed password
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public byte[] UserImage { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    // Signup model
    public class SignupModel : LoginModel
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string UserImage { get; set; }
    }
}
