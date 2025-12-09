using System.Windows;
using System.Windows.Input;
using CaseManagementSystem.ViewModels;
using Telerik.Windows.Controls;

namespace CaseManagementSystem.Views.Authentication
{
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
            StyleManager.SetTheme(this, new MaterialTheme());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UsernameBox.Focus(); // Set initial focus to username
        }

        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            var signupWindow = new SignupView();
            signupWindow.Show();
            this.Close();
        }

        private void ForgotPassword_Click(object sender, MouseButtonEventArgs e)
        {
            var forgotWindow = new ForgotPasswordView();
            forgotWindow.Show();
            this.Close();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows[0].Close();
        }
    }
}
//using CaseManagementSystem.Helpers;
//using CaseManagementSystem.ViewModels;
//using System.Windows;
//using System.Windows.Controls;
//using Telerik.Windows.Controls;
//namespace CaseManagementSystem.Views.Authentication
//{
//    public partial class LoginPage : Window
//    {
//        public LoginPage()
//        {
//            InitializeComponent();
//            this.DataContext = new LoginViewModel();
//            StyleManager.SetTheme(this, new FluentTheme());
//        }

//        private void UserControl_Loaded(object sender, RoutedEventArgs e)
//        {
//            UsernameBox.Focus(); // initial focus
//        }

//        private void Signup_Click(object sender, RoutedEventArgs e)
//        {
//            var signupWindow = new SignupView();
//            signupWindow.Show();

//            this.Close(); // close login page completely
//        }

//        private void ForgotPassword_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
//        {
//            var forgotWindow = new ForgotPasswordView();
//            forgotWindow.Show();  // modal window

//            // After forgot password window closes, show login window again
//            this.Close();
//        }
//    }
//}

////using CaseManagementSystem.Helpers;
////using CaseManagementSystem.ViewModels;
////using System.Windows;
////using System.Windows.Controls;

////namespace CaseManagementSystem.Views.Authentication
////{
////    public partial class LoginPage : Window
////    {
////        public LoginPage()
////        {
////            InitializeComponent();
////            this.DataContext = new LoginViewModel();
////        }
////        private void UserControl_Loaded(object sender, RoutedEventArgs e)
////        {
////            UsernameBox.Focus();   // initial focus
////        }
////        private void Signup_Click(object sender, RoutedEventArgs e)
////        {
////            this.Visibility = Visibility.Hidden;

////            var signupWindow = new SignupView();
////            signupWindow.Show();
////            this.Visibility = Visibility.Visible;
////        }

////        private void ForgotPassword_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
////        {
////            this.Visibility = Visibility.Hidden;

////            ForgotPasswordView window = new ForgotPasswordView();
////            window.ShowDialog();

////            this.Visibility = Visibility.Visible;
////        }

////    }
////}
