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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Викликаємо подію LoginRequested і передаємо параметр, що вказує на необхідність відкриття сторінки реєстрації
            LoginRequested?.Invoke(this, new EventArgsWithRegistrationPage());
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

    // Клас для передачі параметрів події
    public class EventArgsWithRegistrationPage : EventArgs
    {
        public bool OpenRegistrationPage { get; set; } = true;
    }
}
