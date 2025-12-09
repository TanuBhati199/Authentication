using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace CaseManagementSystem.Helpers
{
    public static class PasswordHelper
    {
        public static readonly DependencyProperty BoundPassword =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordHelper),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject obj)
            => (string)obj.GetValue(BoundPassword);

        public static void SetBoundPassword(DependencyObject obj, string value)
            => obj.SetValue(BoundPassword, value);

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RadPasswordBox pb)
            {
                pb.PasswordChanged -= Pb_PasswordChanged;

                if (pb.Password != (string)e.NewValue)
                    pb.Password = (string)e.NewValue;

                pb.PasswordChanged += Pb_PasswordChanged;
            }
        }

        private static void Pb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is RadPasswordBox pb)
            {
                SetBoundPassword(pb, pb.Password);
            }
        }
    }
}
