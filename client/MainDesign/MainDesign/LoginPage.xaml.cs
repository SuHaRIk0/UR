using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MainDesign
{
    public partial class LoginPage : Page
    {
        public event EventHandler LoginRequested;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            LoginRequested?.Invoke(this, new EventArgsWithRegistrationPage());
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

    public class EventArgsWithRegistrationPage : EventArgs
    {
        public bool OpenRegistrationPage { get; set; } = true;
        public bool OpenLoginPage { get; set; } = false; 
    }
}
