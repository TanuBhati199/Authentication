using System.Windows;
using CaseManagementSystem.Views.Authentication;
using Telerik.Windows.Controls;
namespace CaseManagementSystem
{
    public partial class App : Application
    {
        //private void Application_Startup(object sender, StartupEventArgs e)
        //{
        //    SetTheme("Material");   // default theme
        //}

        //private void SetTheme(string themeName)
        //{
        //    StyleManager.ApplicationTheme = (Theme)Activator.CreateInstance(
        //        Type.GetType("Telerik.Windows.Controls." + themeName + "Theme"));
        //}
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginWindow = new LoginPage(); // THIS MUST BE A WINDOW
            loginWindow.Show();
        }
    }
}
