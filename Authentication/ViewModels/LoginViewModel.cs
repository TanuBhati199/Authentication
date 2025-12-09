using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using CaseManagementSystem.Helpers;
using CaseManagementSystem.Services;
using CaseManagementSystem.Settings;
using CaseManagementSystem.Views.Dashboards;
using Telerik.Windows.Controls;

namespace CaseManagementSystem.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _rememberMe;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public bool RememberMe
        {
            get => _rememberMe;
            set
            {
                _rememberMe = value;
                OnPropertyChanged(nameof(RememberMe));
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(_ => ExecuteLogin());
            LoadRememberedCredentials();
        }

        private void LoadRememberedCredentials()
        {
            if (Properties.Settings.Default.RememberMe)
            {
                Username = Properties.Settings.Default.SavedUsername;
                Password = Properties.Settings.Default.SavedPassword;
                RememberMe = true;
            }
        }

        private void ExecuteLogin()
        {
            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Username & Password are required.";
                return;
            }

            try
            {
                using SqlConnection conn = new SqlConnection(AppValues.ConString);
                conn.Open();

                using SqlCommand cmd = new SqlCommand(
                    "SELECT PasswordHash, IsActive,Role FROM Users WHERE Username=@User",
                    conn
                );
                cmd.Parameters.AddWithValue("@User", Username);

                using SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    ErrorMessage = "Invalid username or password.";
                    return;
                }

                string storedHash = reader["PasswordHash"].ToString();
                bool isActive = Convert.ToBoolean(reader["IsActive"]);
                string role = reader["Role"].ToString();
                if (!SecurityHelper.Verify(Password, storedHash))
                {
                    ErrorMessage = "Invalid username or password.";
                    return;
                }

                if (!isActive)
                {
                    ErrorMessage = "Your account is deactivated. Contact admin.";
                    return;
                }

                SaveRememberMe();

                //MessageBox.Show($"Welcome, {Username}!", "Login Success");
                //MessageBox.Show("Login Success!");
                OpenDashboard(role);
                Application.Current.Windows[0].Close();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Login failed: " + ex.Message;
            }
        }

        private void OpenDashboard(string role)
        {
            Window dashboard = null;

            switch (role.ToLower())
            {
                case "admin":
                    dashboard = new AdminDashboard();
                    break;

                case "analyst":
                    dashboard = new AnalystDashboard();
                    break;

                case "superadmin":
                    dashboard = new SuperAdminDashboard();
                    break;

                case "teamlead":
                case "team lead":
                    dashboard = new TeamLeadDashboard();
                    break;

                default:
                    RadWindow.Alert("Unknown role. Contact system administrator.");
                    return;
            }

            dashboard.Show();
        }

        private void SaveRememberMe()
        {
            if (RememberMe)
            {
                Properties.Settings.Default.SavedUsername = Username;
                Properties.Settings.Default.SavedPassword = Password;
                Properties.Settings.Default.RememberMe = true;
            }
            else
            {
                Properties.Settings.Default.SavedUsername = "";
                Properties.Settings.Default.SavedPassword = "";
                Properties.Settings.Default.RememberMe = false;
            }
            Properties.Settings.Default.Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

//using CaseManagementSystem.Helpers;
//using CaseManagementSystem.Services;
//using CaseManagementSystem.Views.Dashboards;
//using System;
//using System.ComponentModel;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Input;

//namespace CaseManagementSystem.ViewModels
//{
//    public class LoginViewModel : INotifyPropertyChanged
//    {
//        private string _username;
//        private string _password;
//        private bool _rememberMe;
//        private string _errorMessage;

//        private readonly AuthService _authService;

//        public string Username
//        {
//            get => _username;
//            set { _username = value; OnPropertyChanged(nameof(Username)); }
//        }

//        public string Password
//        {
//            get => _password;
//            set { _password = value; OnPropertyChanged(nameof(Password)); }
//        }

//        public bool RememberMe
//        {
//            get => _rememberMe;
//            set { _rememberMe = value; OnPropertyChanged(nameof(RememberMe)); }
//        }

//        public string ErrorMessage
//        {
//            get => _errorMessage;
//            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
//        }

//        public ICommand LoginCommand { get; }

//        public LoginViewModel()
//        {
//            _authService = new AuthService();
//            LoginCommand = new RelayCommand(_ => ExecuteLogin());
//            LoadRememberedCredentials();
//        }

//        private void LoadRememberedCredentials()
//        {
//            try
//            {
//                if (Properties.Settings.Default.RememberMe)
//                {
//                    Username = Properties.Settings.Default.SavedUsername;
//                    Password = Properties.Settings.Default.SavedPassword;
//                    RememberMe = true;
//                }
//            }
//            catch
//            {
//                ErrorMessage = "Failed to load saved credentials.";
//            }
//        }

//        private async Task ExecuteLogin()
//        {
//            ErrorMessage = "";

//            // ---- VALIDATION ----
//            if (string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Password))
//            {
//                    ErrorMessage = "Username & Password is required.";
//                    return;
//                        }
//            else if (string.IsNullOrWhiteSpace(Username))
//            {
//                ErrorMessage = "Username is required.";
//                return;
//            }

//            else if(string.IsNullOrWhiteSpace(Password))
//            {
//                ErrorMessage = "Password is required.";
//                return;
//            }

//            try
//            {
//                Mouse.OverrideCursor = Cursors.Wait;
//                var user = _authService.Login(Username, Password);

//                if (user == null)
//                {
//                    ErrorMessage = "Invalid username or password.";
//                    return;
//                }

//                if (!user.IsActive)
//                {
//                    ErrorMessage = "Your account is deactivated. Contact admin.";
//                    return;
//                }

//                // SAVE REMEMBER ME
//                SaveRememberMe(Username, Password);

//                MessageBox.Show($"Welcome, {user.Username}!", "Success");
//                }
//            catch (Exception ex)
//            {
//                ErrorMessage = "Login failed: " + ex.Message;
//            }
//            finally
//            {
//                Mouse.OverrideCursor = null;

//            }
//        }

//        private void SaveRememberMe(string user, string pass)
//        {
//            try
//            {
//                if (RememberMe)
//                {
//                    Properties.Settings.Default.SavedUsername = user;
//                    Properties.Settings.Default.SavedPassword = pass;
//                    Properties.Settings.Default.RememberMe = true;
//                }
//                else
//                {
//                    Properties.Settings.Default.SavedUsername = "";
//                    Properties.Settings.Default.SavedPassword = "";
//                    Properties.Settings.Default.RememberMe = false;
//                }
//                Properties.Settings.Default.Save();
//            }
//            catch
//            {
//                ErrorMessage = "Failed to save login preferences.";
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;
//        private void OnPropertyChanged(string p) =>
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
//    }
//}
