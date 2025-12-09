using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CaseManagementSystem.Helpers;
using CaseManagementSystem.ViewModels;
using Telerik.Windows.Controls;

namespace CaseManagementSystem.Views.Authentication
{
    public partial class SignupView : Window
    {
        public SignupView()
        {
            InitializeComponent();
            StyleManager.SetTheme(this, new MaterialTheme());
        }

        // Allow window dragging from title bar
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        // Minimize button handler
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Close button handler
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SignInHyperlink_Click(object sender, RoutedEventArgs e)
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
    }

    //    private void Email_LostFocus(object sender, RoutedEventArgs e)
    //    {
    //        var vm = (SignupViewModel)DataContext;
    //        if (string.IsNullOrWhiteSpace(EmailInput.Text))
    //        {
    //            vm.EmailError = "Email is required.";
    //            RadWindow.Alert("Email is required.");
    //            return;
    //        }

    //        if (
    //            !System.Text.RegularExpressions.Regex.IsMatch(
    //                EmailInput.Text,
    //                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
    //            )
    //        )
    //        {
    //            vm.EmailError = "Enter a valid email address.";
    //            RadWindow.Alert("Enter a valid email address.");
    //        }
    //        else
    //        {
    //            vm.EmailError = "";
    //        }
    //    }

    //    private void Mobile_LostFocus(object sender, RoutedEventArgs e)
    //    {
    //        var vm = (SignupViewModel)DataContext;
    //        if (string.IsNullOrWhiteSpace(MobileInput.Text))
    //        {
    //            vm.MobileError = "Mobile number is required.";
    //            RadWindow.Alert("Mobile number is required.");
    //            return;
    //        }

    //        if (MobileInput.Text.Length != 10)
    //        {
    //            vm.MobileError = "Mobile number must be 10 digits.";
    //            RadWindow.Alert("Mobile number must be 10 digits (Indian format).");
    //        }
    //        else
    //        {
    //            vm.MobileError = "";
    //        }
    //    }

    //    private void Password_LostFocus(object sender, RoutedEventArgs e)
    //    {
    //        var vm = (SignupViewModel)DataContext;
    //        if (string.IsNullOrWhiteSpace(PasswordInput.Password))
    //        {
    //            vm.PasswordError = "Password is required.";
    //            RadWindow.Alert("Password is required.");
    //            return;
    //        }

    //        // Updated regex: letters + digits required, special characters allowed
    //        if (
    //            !System.Text.RegularExpressions.Regex.IsMatch(
    //                PasswordInput.Password,
    //                @"^(?=.*[A-Za-z])(?=.*\d).{6,}$"
    //            )
    //        )
    //        {
    //            vm.PasswordError =
    //                "Password must contain letters, numbers and be at least 6 characters.";
    //            RadWindow.Alert(
    //                "Password must contain letters, numbers and be at least 6 characters."
    //            );
    //        }
    //        else
    //        {
    //            vm.PasswordError = "";
    //        }
    //    }

    //    private void ConfirmPassword_LostFocus(object sender, RoutedEventArgs e)
    //    {
    //        var vm = (SignupViewModel)DataContext;
    //        if (ConfirmPasswordInput.Password != PasswordInput.Password)
    //        {
    //            vm.ConfirmPasswordError = "Passwords do not match.";
    //            RadWindow.Alert("Confirm Password does not match Password.");
    //        }
    //        else
    //        {
    //            vm.ConfirmPasswordError = "";
    //        }
    //    }

    //    private void Mobile_PreviewTextInput(object sender, TextCompositionEventArgs e)
    //    {
    //        e.Handled = !e.Text.All(char.IsDigit);
    //    }
    //}

    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string)
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            return null;
        }
    }
}
//using CaseManagementSystem.Helpers;
//using System.Windows;
//using System.Windows.Controls;
//using Telerik.Windows.Controls;
//namespace CaseManagementSystem.Views.Authentication
//{
//    public partial class SignupView : Window
//    {
//        public SignupView()
//        {
//            InitializeComponent();
//            StyleManager.SetTheme(this, new MaterialTheme());
//        }

//    }
//}
