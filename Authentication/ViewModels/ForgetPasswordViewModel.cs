using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using CaseManagementSystem.Helpers;
using CaseManagementSystem.Settings;
using CaseManagementSystem.Views.Authentication;
using Telerik.Windows.Controls;

namespace CaseManagementSystem.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        public ForgotPasswordViewModel()
        {
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

            VerifyCommand = new RelayCommand(VerifyUser);
            ResetPasswordCommand = new RelayCommand(ResetPassword);
            BackToLoginCommand = new RelayCommand(BackToLogin);
        }

        #region Properties
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
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

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged();
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
            }
        }

        private bool _canReset;
        public bool CanReset
        {
            get => _canReset;
            set
            {
                _canReset = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> SecurityQuestions { get; set; }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand VerifyCommand { get; set; }
        public ICommand ResetPasswordCommand { get; set; }
        public ICommand BackToLoginCommand { get; set; }
        #endregion

        #region Methods

        private void VerifyUser(object obj)
        {
            if (
                string.IsNullOrWhiteSpace(Username)
                || string.IsNullOrWhiteSpace(SelectedSecurityQuestion)
                || string.IsNullOrWhiteSpace(SecurityAnswer)
            )
            {
                StatusMessage = "Please fill all required fields.";
                return;
            }

            try
            {
                using SqlConnection conn = new SqlConnection(AppValues.ConString);
                conn.Open();

                using SqlCommand cmd = new SqlCommand(
                    "SELECT SecurityAnswer FROM Users WHERE Username=@User AND SecurityQuestion=@Question",
                    conn
                );
                cmd.Parameters.AddWithValue("@User", Username);
                cmd.Parameters.AddWithValue("@Question", SelectedSecurityQuestion);

                object result = cmd.ExecuteScalar();

                if (result == null)
                {
                    StatusMessage = "Invalid username or security question.";
                    CanReset = false;
                    return;
                }

                string storedHash = result.ToString();

                if (!SecurityHelper.Verify(SecurityAnswer, storedHash))
                {
                    StatusMessage = "Incorrect security answer.";
                    CanReset = false;
                    return;
                }

                StatusMessage = "Verification successful. You can reset your password.";
                CanReset = true;
            }
            catch (Exception ex)
            {
                StatusMessage = "Error during verification: " + ex.Message;
                CanReset = false;
            }
        }

        private void ResetPassword(object obj)
        {
            if (!CanReset)
            {
                StatusMessage = "Please verify your identity first.";
                return;
            }

            if (
                string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmPassword)
            )
            {
                StatusMessage = "Please enter new password and confirm it.";
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                StatusMessage = "Passwords do not match.";
                return;
            }

            string hashedPassword = SecurityHelper.Hash(NewPassword);

            try
            {
                using SqlConnection conn = new SqlConnection(AppValues.ConString);
                conn.Open();

                using SqlCommand cmd = new SqlCommand(
                    "UPDATE Users SET PasswordHash=@Password WHERE Username=@User",
                    conn
                );
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@User", Username);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    //RadWindow.Alert(
                    //    new DialogParameters
                    //    {
                    //        Header = "Success",
                    //        Content =
                    //            "Password reset successfully! You can now login with your new password.",
                    //        OkButtonContent = "OK",
                    //    }
                    //);
                    MessageBox.Show(
                        "Password reset successfully! Login with new password.",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    // Close current window and show login
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Window currentWindow = Application
                            .Current.Windows.OfType<Window>()
                            .FirstOrDefault(w => w.IsActive);
                        var loginPage = new LoginPage();
                        loginPage.Show();
                        currentWindow?.Close();
                    });
                }
                else
                {
                    StatusMessage = "Failed to reset password.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Error resetting password: " + ex.Message;
            }
        }

        private void BackToLogin(object obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Window currentWindow = Application
                    .Current.Windows.OfType<Window>()
                    .FirstOrDefault(w => w.IsActive);
                var loginPage = new LoginPage();
                loginPage.Show();
                currentWindow?.Close();
            });
        }

        #endregion
    }
}
//using CaseManagementSystem.Helpers;
//using CaseManagementSystem.Settings;
//using CaseManagementSystem.Views.Authentication;
//using System;
//using System.Collections.ObjectModel;
//using System.Data;
//using System.Data.SqlClient;
//using System.Windows;
//using System.Windows.Input;

//namespace CaseManagementSystem.ViewModels
//{
//    public class ForgotPasswordViewModel : BaseViewModel
//    {
//        public ForgotPasswordViewModel()
//        {
//            SecurityQuestions = new ObservableCollection<string>
//            {
//                "What is your pet's name?",
//                "What city were you born in?",
//                "What is your favorite food?",
//                "What is your mother's maiden name?",
//                 "What was the name of your first school?",
//    "What high school did you attend?",
//    "What was the make and model of your first car?",
//    "What was the name of the street you grew up on?",
//    "What was your childhood best friend's name?",
//    "In what city did your parents meet?",
//    "What was the name of your favorite stuffed animal?",
//    "What is your oldest sibling's middle name?",
//    "What was the first concert you attended?"
//            };

//            VerifyCommand = new RelayCommand(VerifyUser);
//            ResetPasswordCommand = new RelayCommand(ResetPassword);
//        }

//        #region Properties
//        private string _username;
//        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }

//        private string _selectedSecurityQuestion;
//        public string SelectedSecurityQuestion { get => _selectedSecurityQuestion; set { _selectedSecurityQuestion = value; OnPropertyChanged(); } }

//        private string _securityAnswer;
//        public string SecurityAnswer { get => _securityAnswer; set { _securityAnswer = value; OnPropertyChanged(); } }

//        private string _newPassword;
//        public string NewPassword { get => _newPassword; set { _newPassword = value; OnPropertyChanged(); } }

//        private string _confirmPassword;
//        public string ConfirmPassword { get => _confirmPassword; set { _confirmPassword = value; OnPropertyChanged(); } }

//        private bool _canReset;
//        public bool CanReset { get => _canReset; set { _canReset = value; OnPropertyChanged(); } }

//        public ObservableCollection<string> SecurityQuestions { get; set; }

//        private string _statusMessage;
//        public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(); } }
//        #endregion

//        #region Commands
//        public ICommand VerifyCommand { get; set; }
//        public ICommand ResetPasswordCommand { get; set; }
//        #endregion

//        #region Methods

//        private void VerifyUser(object obj)
//        {
//            if (string.IsNullOrWhiteSpace(Username) ||
//                string.IsNullOrWhiteSpace(SelectedSecurityQuestion) ||
//                string.IsNullOrWhiteSpace(SecurityAnswer))
//            {
//                StatusMessage = "Please fill all required fields.";
//                return;
//            }

//            try
//            {
//                using SqlConnection conn = new SqlConnection(AppValues.ConString);
//                conn.Open();

//                using SqlCommand cmd = new SqlCommand(
//                    "SELECT SecurityAnswer FROM Users WHERE Username=@User AND SecurityQuestion=@Question", conn);
//                cmd.Parameters.AddWithValue("@User", Username);
//                cmd.Parameters.AddWithValue("@Question", SelectedSecurityQuestion);

//                object result = cmd.ExecuteScalar();

//                if (result == null)
//                {
//                    StatusMessage = "Invalid username or security question.";
//                    CanReset = false;
//                    return;
//                }

//                string storedHash = result.ToString();

//                if (!SecurityHelper.Verify(SecurityAnswer, storedHash))
//                {
//                    StatusMessage = "Incorrect security answer.";
//                    CanReset = false;
//                    return;
//                }

//                StatusMessage = "Verification successful. You can reset your password.";
//                CanReset = true;
//            }
//            catch (Exception ex)
//            {
//                StatusMessage = "Error during verification: " + ex.Message;
//                CanReset = false;
//            }
//        }

//        private void ResetPassword(object obj)
//        {
//            if (!CanReset)
//            {
//                StatusMessage = "Please verify your identity first.";
//                return;
//            }

//            if (string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmPassword))
//            {
//                StatusMessage = "Please enter new password and confirm it.";
//                return;
//            }

//            if (NewPassword != ConfirmPassword)
//            {
//                StatusMessage = "Passwords do not match.";
//                return;
//            }

//            string hashedPassword = SecurityHelper.Hash(NewPassword);

//            try
//            {
//                using SqlConnection conn = new SqlConnection(AppValues.ConString);
//                conn.Open();

//                using SqlCommand cmd = new SqlCommand(
//                    "UPDATE Users SET PasswordHash=@Password WHERE Username=@User", conn);
//                cmd.Parameters.AddWithValue("@Password", hashedPassword);
//                cmd.Parameters.AddWithValue("@User", Username);

//                int rows = cmd.ExecuteNonQuery();
//                if (rows > 0)
//                {
//                    StatusMessage = "Password reset successfully!";
//                    CanReset = false;

//                    Username = SelectedSecurityQuestion = SecurityAnswer = NewPassword = ConfirmPassword = "";
//                    Application.Current.Dispatcher.Invoke(() =>
//                    {
//                        var login = new LoginPage();
//                        login.Show();
//                    });
//                }
//                else
//                {
//                    StatusMessage = "Failed to reset password.";
//                }
//            }
//            catch (Exception ex)
//            {
//                StatusMessage = "Error resetting password: " + ex.Message;
//            }
//        }
//        #endregion
//    }
//}
