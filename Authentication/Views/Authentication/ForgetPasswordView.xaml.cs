using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaseManagementSystem.ViewModels;
using Telerik.Windows.Controls;

namespace CaseManagementSystem.Views.Authentication
{
    public partial class ForgotPasswordView : Window
    {
        public ForgotPasswordView()
        {
            InitializeComponent();
            StyleManager.SetTheme(this, new MaterialTheme());
        }

        private void NewPwdBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ForgotPasswordViewModel vm)
            {
                vm.NewPassword = ((PasswordBox)sender).Password;
            }
        }

        private void ConfirmPwdBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ForgotPasswordViewModel vm)
            {
                vm.ConfirmPassword = ((PasswordBox)sender).Password;
            }
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
//using CaseManagementSystem.ViewModels;
//using System.Windows;
//using System.Windows.Controls;
//using Telerik.Windows.Controls;
//namespace CaseManagementSystem.Views.Authentication
//{
//    public partial class ForgotPasswordView : Window
//    {
//        public ForgotPasswordView()
//        {
//            InitializeComponent();
//            StyleManager.SetTheme(this, new MaterialTheme());
//        }

//        private void NewPwdBox_OnPasswordChanged(object sender, System.Windows.RoutedEventArgs e)
//        {
//            if (DataContext is ForgotPasswordViewModel vm)
//            {
//                vm.NewPassword = ((PasswordBox)sender).Password;
//            }
//        }

//        private void ConfirmPwdBox_OnPasswordChanged(object sender, System.Windows.RoutedEventArgs e)
//        {
//            if (DataContext is ForgotPasswordViewModel vm)
//            {
//                vm.ConfirmPassword = ((PasswordBox)sender).Password;
//            }
//        }
//    }
//}
