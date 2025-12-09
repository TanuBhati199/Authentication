using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CaseManagementSystem.Helpers;
using CaseManagementSystem.Models;
using CaseManagementSystem.Settings;
using CaseManagementSystem.Views.Authentication;
using static CaseManagementSystem.Settings.AppValues;

namespace CaseManagementSystem.ViewModels
{
    public class SignupViewModel : BaseViewModel
    {
        public SignupViewModel()
        {
            UserId = Guid.NewGuid().ToString().Substring(0, 8);

            UserTypes = new List<string> { "Admin", "Team Lead", "Analyst" };
            WorkUnderList = new List<string> { "Admin", "Team Lead", "Analyst" };
            Genders = new List<string> { "Male", "Female", "Other" };

            SecurityQuestions = new ObservableCollection<string>
            {
                "What is your pet's name?",
                "What city were you born in?",
                "What is your favorite food?",
                "What was the name of your first school?",
                "What high school did you attend?",
                "What was the make and model of your first car?",
                "What was your childhood best friend's name?",
                "What was the name of your favorite stuffed animal?",
            };

            RegisterCommand = new RelayCommand(RegisterUser);
            ChooseImageCommand = new RelayCommand(ChooseImage);
        }

        public string UserId { get; set; }
        public List<string> UserTypes { get; set; }
        public List<string> WorkUnderList { get; set; }
        public List<string> Genders { get; set; }
        public ObservableCollection<string> SecurityQuestions { get; set; }

        public ICommand RegisterCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }

        #region Properties
        public string EmailError { get; set; }
        public string MobileError { get; set; }
        public string PasswordError { get; set; }
        public string ConfirmPasswordError { get; set; }
        private bool _isFormValid;
        public bool IsFormValid
        {
            get => _isFormValid;
            set
            {
                _isFormValid = value;
                OnPropertyChanged();
            }
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
                Validate();
            }
        }

        private string _mobile;
        public string Mobile
        {
            get => _mobile;
            set
            {
                _mobile = value;
                OnPropertyChanged();
                Validate();
            }
        }

        private string _selectedUserType;
        public string SelectedUserType
        {
            get => _selectedUserType;
            set
            {
                _selectedUserType = value;
                OnPropertyChanged();
            }
        }

        private string _selectedWorkUnder;
        public string SelectedWorkUnder
        {
            get => _selectedWorkUnder;
            set
            {
                _selectedWorkUnder = value;
                OnPropertyChanged();
            }
        }

        private string _department;
        public string Department
        {
            get => _department;
            set
            {
                _department = value;
                OnPropertyChanged();
            }
        }

        private string _designation;
        public string Designation
        {
            get => _designation;
            set
            {
                _designation = value;
                OnPropertyChanged();
            }
        }

        private string _selectedGender;
        public string SelectedGender
        {
            get => _selectedGender;
            set
            {
                _selectedGender = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _dateOfJoining;
        public DateTime? DateOfJoining
        {
            get => _dateOfJoining;
            set
            {
                _dateOfJoining = value;
                OnPropertyChanged();
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                Validate();
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
                Validate();
            }
        }

        private string _selectedSecurityQuestion;
        public string SelectedSecurityQuestion
        {
            get => _selectedSecurityQuestion;
            set
            {
                _selectedSecurityQuestion = value;
                OnPropertyChanged();
            }
        }

        private string _securityAnswer;
        public string SecurityAnswer
        {
            get => _securityAnswer;
            set
            {
                _securityAnswer = value;
                OnPropertyChanged();
            }
        }

        private byte[] _userImageBytes;
        public byte[] UserImageBytes
        {
            get => _userImageBytes;
            set
            {
                _userImageBytes = value;
                OnPropertyChanged();
            }
        }

        private ImageSource _userImage;
        public ImageSource UserImage
        {
            get => _userImage;
            set
            {
                _userImage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public bool Validate()
        {
            EmailError = MobileError = PasswordError = ConfirmPasswordError = "";
            bool isValid = true;

            // Email
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Email is required.";
                isValid = false;
            }
            else if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                EmailError = "Enter a valid email.";
                isValid = false;
            }

            // Mobile
            if (string.IsNullOrWhiteSpace(Mobile))
            {
                MobileError = "Mobile is required.";
                isValid = false;
            }
            else if (Mobile.Length != 10)
            {
                MobileError = "Mobile must be 10 digits.";
                isValid = false;
            }

            // Password
            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Password is required.";
                isValid = false;
            }
            else if (!Regex.IsMatch(Password, @"^(?=.*[A-Za-z])(?=.*\d).{6,}$"))
            {
                PasswordError = "Must contain letters + numbers (min 6 chars).";
                isValid = false;
            }

            // Confirm Password
            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ConfirmPasswordError = "Confirm Password is required.";
                isValid = false;
            }
            else if (ConfirmPassword != Password)
            {
                ConfirmPasswordError = "Passwords do not match.";
                isValid = false;
            }

            // Set global form validity
            IsFormValid = isValid;

            return isValid;
        }

        private void RegisterUser(object obj)
        { // Validate individual fields
            if (!Validate())
            {
                MessageBox.Show(
                    "Please correct the highlighted errors before submitting.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            if (string.IsNullOrWhiteSpace(UserName))
            {
                MessageBox.Show("User Name is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Email is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Mobile))
            {
                MessageBox.Show("Mobile number is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedUserType))
            {
                MessageBox.Show("User Type must be selected.");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedGender))
            {
                MessageBox.Show("Gender must be selected.");
                return;
            }

            if (DateOfJoining == null)
            {
                MessageBox.Show("Date Of Joining is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                MessageBox.Show("Address is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Password is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                MessageBox.Show("Confirm Password is required.");
                return;
            }

            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Password and Confirm Password do not match.");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedSecurityQuestion))
            {
                MessageBox.Show("Please select a Security Question.");
                return;
            }

            if (string.IsNullOrWhiteSpace(SecurityAnswer))
            {
                MessageBox.Show("Security Answer is required.");
                return;
            }

            // Validation
            ////if (string.IsNullOrWhiteSpace(UserName) ||
            ////    string.IsNullOrWhiteSpace(Email) ||
            ////    string.IsNullOrWhiteSpace(Mobile) ||
            ////    string.IsNullOrWhiteSpace(SelectedUserType) ||
            ////    string.IsNullOrWhiteSpace(SelectedGender) ||
            ////    string.IsNullOrWhiteSpace(Address) ||
            ////    string.IsNullOrWhiteSpace(Password) ||
            ////    string.IsNullOrWhiteSpace(ConfirmPassword) ||
            ////    string.IsNullOrWhiteSpace(SelectedSecurityQuestion)||
            ////    string.IsNullOrWhiteSpace(SecurityAnswer) ||
            ////    DateOfJoining == null)
            ////{
            ////    MessageBox.Show("Please fill all required fields.");
            ////    return;
            ////}

            ////if (Password != ConfirmPassword)
            ////{
            ////    MessageBox.Show("Password and Confirm Password do not match.");
            ////    return;
            ////}

            string passwordHash = SecurityHelper.Hash(Password);
            string answerHash = SecurityHelper.Hash(SecurityAnswer);

            try
            {
                using SqlConnection conn = new SqlConnection(AppValues.ConString);
                conn.Open();

                using SqlCommand cmd = new SqlCommand("sp_InsertUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", UserName);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Mobile", Mobile);
                cmd.Parameters.AddWithValue("@Role", SelectedUserType);
                cmd.Parameters.AddWithValue("@Gender", SelectedGender);
                cmd.Parameters.AddWithValue("@DateOfJoining", DateOfJoining);
                cmd.Parameters.AddWithValue("@Address", Address);
                cmd.Parameters.AddWithValue("@Department", Department ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Designation", Designation ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SecurityQuestion", SelectedSecurityQuestion);
                cmd.Parameters.AddWithValue("@SecurityAnswer", answerHash);
                cmd.Parameters.Add("@UserImage", SqlDbType.VarBinary).Value =
                    (object)UserImageBytes ?? DBNull.Value;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating account: " + ex.Message);
                return;
            }
            MessageBox.Show(
                "Account created successfully!",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
            Application.Current.Dispatcher.Invoke(() =>
            {
                Window currentWindow = Application
                    .Current.Windows.OfType<Window>()
                    .FirstOrDefault(w => w.IsActive);
                var loginPage = new LoginPage();
                loginPage.Show();
                currentWindow?.Close();
            });
            ClearForm();
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    var login = new LoginPage();
            //    login.Show();
            //});
        }

        private void ChooseImage(object obj)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.png;*.jpeg",
            };
            if (dialog.ShowDialog() == true)
            {
                UserImageBytes = File.ReadAllBytes(dialog.FileName);
                UserImage = LoadImage(UserImageBytes);
            }
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private void ClearForm()
        {
            UserName =
                Email =
                Mobile =
                Address =
                Department =
                Designation =
                Password =
                ConfirmPassword =
                    "";
            SelectedUserType =
                SelectedWorkUnder =
                SelectedGender =
                SelectedSecurityQuestion =
                SecurityAnswer =
                    null;
            DateOfJoining = null;
            UserImage = null;
            UserImageBytes = null;
        }

        #endregion
    }
}

//using CaseManagementSystem.Helpers;
//using CaseManagementSystem.Models;
//using CaseManagementSystem.Settings;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Data;
//using System.Data.SqlClient;
//using System.IO;
//using System.Security.Cryptography;
//using System.Text;
//using System.Windows;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using static CaseManagementSystem.Settings.AppValues;

//namespace CaseManagementSystem.ViewModels
//{
//    public class SignupViewModel : BaseViewModel
//    {
//        public SignupViewModel()
//        {
//            UserId = Guid.NewGuid().ToString().Substring(0, 8);

//            UserTypes = new List<string> { "Admin", "Team Lead", "Analyst" };
//            WorkUnderList = new List<string> { "Admin", "Team Lead", "Analyst" };
//            Genders = new List<string> { "Male", "Female", "Other" };

//            RegisterCommand = new RelayCommand(RegisterUser);
//            ChooseImageCommand = new RelayCommand(ChooseImage);
//        }

//        public string UserId { get; set; }

//        private string _userName;
//        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(); } }

//        private string _email;
//        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }

//        private string _mobile;
//        public string Mobile { get => _mobile; set { _mobile = value; OnPropertyChanged(); } }

//        public List<string> UserTypes { get; set; }

//        private string _selectedUserType;
//        public string SelectedUserType { get => _selectedUserType; set { _selectedUserType = value; OnPropertyChanged(); } }

//        public List<string> WorkUnderList { get; set; }

//        private string _selectedWorkUnder;
//        public string SelectedWorkUnder { get => _selectedWorkUnder; set { _selectedWorkUnder = value; OnPropertyChanged(); } }

//        private string _department;
//        public string Department { get => _department; set { _department = value; OnPropertyChanged(); } }

//        private string _designation;
//        public string Designation { get => _designation; set { _designation = value; OnPropertyChanged(); } }

//        public List<string> Genders { get; set; }

//        private string _selectedGender;
//        public string SelectedGender { get => _selectedGender; set { _selectedGender = value; OnPropertyChanged(); } }

//        private DateTime? _dateOfJoining;
//        public DateTime? DateOfJoining { get => _dateOfJoining; set { _dateOfJoining = value; OnPropertyChanged(); } }

//        private string _address;
//        public string Address { get => _address; set { _address = value; OnPropertyChanged(); } }

//        private string _password;
//        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

//        private string _confirmPassword;
//        public string ConfirmPassword { get => _confirmPassword; set { _confirmPassword = value; OnPropertyChanged(); } }

//        private string _securityQuestion;
//        public string SecurityQuestion { get => _securityQuestion; set { _securityQuestion = value; OnPropertyChanged(); } }

//        private string _securityAnswer;
//        public string SecurityAnswer { get => _securityAnswer; set { _securityAnswer = value; OnPropertyChanged(); } }

//      //  private string _userImage;
//      //  public string UserImage { get => _userImage; set { _userImage = value; OnPropertyChanged(); } }

//        public ICommand RegisterCommand { get; set; }
//        public ICommand ChooseImageCommand { get; set; }
//        public ObservableCollection<string> SecurityQuestions { get; set; } =
//      new ObservableCollection<string>
//      {
//            "What is your pet's name?",
//            "What city were you born in?",
//            "What is your favorite food?",
//            "What is your mother's maiden name?"
//      };
//        private void RegisterUser(object obj)
//        {
//            // Validate required fields
//            if (string.IsNullOrWhiteSpace(UserName) ||
//                string.IsNullOrWhiteSpace(Email) ||
//                string.IsNullOrWhiteSpace(Mobile) ||
//                string.IsNullOrWhiteSpace(SelectedUserType) ||
//                string.IsNullOrWhiteSpace(SelectedGender) ||
//                string.IsNullOrWhiteSpace(Address) ||
//                string.IsNullOrWhiteSpace(Password) ||
//                string.IsNullOrWhiteSpace(ConfirmPassword) ||
//                DateOfJoining == null)
//            {
//                MessageBox.Show("Please fill all required fields.");
//                return;
//            }

//            if (Password != ConfirmPassword)
//            {
//                MessageBox.Show("Password and Confirm Password do not match.");
//                return;
//            }

//            // Hash password
//            string hashedPassword = ComputeSha256Hash(Password);

//            try
//            {
//                using (SqlConnection conn = new SqlConnection(AppValues.ConString))
//                {
//                    conn.Open();
//                    using (SqlCommand cmd = new SqlCommand("sp_InsertUser", conn))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;

//                        cmd.Parameters.AddWithValue("@Username", UserName);
//                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
//                        cmd.Parameters.AddWithValue("@Email", Email);
//                        cmd.Parameters.AddWithValue("@Mobile", Mobile);
//                        cmd.Parameters.AddWithValue("@Role", SelectedUserType);
//                        cmd.Parameters.AddWithValue("@Gender", SelectedGender);
//                        cmd.Parameters.AddWithValue("@DateOfJoining", DateOfJoining);
//                        cmd.Parameters.AddWithValue("@Address", Address);
//                        cmd.Parameters.AddWithValue("@Department", Department ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@Designation", Designation ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@SecurityQuestion", SecurityQuestion ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@SecurityAnswer", SecurityAnswer ?? (object)DBNull.Value);
//                        cmd.Parameters.Add("@UserImage", SqlDbType.VarBinary).Value =
//    (object)UserImageBytes ?? DBNull.Value;

//                        cmd.ExecuteNonQuery();
//                    }
//                }

//                MessageBox.Show("Account created successfully!");
//                ClearForm();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Error creating account: " + ex.Message);
//            }
//        }

//        // For database
//        private byte[] _userImageBytes;
//        public byte[] UserImageBytes
//        {
//            get => _userImageBytes;
//            set { _userImageBytes = value; OnPropertyChanged(); }
//        }

//        // For UI binding
//        private ImageSource _userImage;
//        public ImageSource UserImage
//        {
//            get => _userImage;
//            set { _userImage = value; OnPropertyChanged(); }
//        }

//        private void ChooseImage(object obj)
//        {
//            var dialog = new Microsoft.Win32.OpenFileDialog();
//            dialog.Filter = "Image Files|*.jpg;*.png;*.jpeg";
//            if (dialog.ShowDialog() == true)
//            {
//                // Read file bytes for database
//                UserImageBytes = File.ReadAllBytes(dialog.FileName);

//                // Convert bytes to ImageSource for display
//                UserImage = LoadImage(UserImageBytes);
//            }
//        }

//        // Helper to convert byte[] to BitmapImage
//        private BitmapImage LoadImage(byte[] imageData)
//        {
//            if (imageData == null || imageData.Length == 0) return null;
//            var image = new BitmapImage();
//            using (var mem = new MemoryStream(imageData))
//            {
//                mem.Position = 0;
//                image.BeginInit();
//                image.CacheOption = BitmapCacheOption.OnLoad;
//                image.StreamSource = mem;
//                image.EndInit();
//            }
//            image.Freeze();
//            return image;
//        }

//        private void ClearForm()
//        {
//            UserName = Email = Mobile = Address = Department = Designation =
//     Password = ConfirmPassword = SecurityQuestion = SecurityAnswer = "";

//            SelectedUserType = SelectedWorkUnder = SelectedGender = null;
//            DateOfJoining = null;

//            UserImage = null;
//            _userImageBytes = null;
//        }

//        private string ComputeSha256Hash(string rawData)
//        {
//            using (SHA256 sha256Hash = SHA256.Create())
//            {
//                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
//                StringBuilder builder = new StringBuilder();
//                foreach (byte b in bytes)
//                    builder.Append(b.ToString("x2"));
//                return builder.ToString();
//            }
//        }
//    }
//}
