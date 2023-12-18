using System.Windows;
using System.Windows.Controls;

using YouAre.MVVM.Model;


namespace YouAre.MVVM.View
{
    public partial class Register
    {
        private readonly Frame _navFrame;
        private readonly Server _server;

        public Register(Frame navFrame, Server server)
        {
            InitializeComponent();
            _navFrame = navFrame;
            _server = server;
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            var login = new Login(_navFrame, _server);
            _navFrame.Navigate(login);
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
