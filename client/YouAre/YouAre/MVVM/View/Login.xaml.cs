using System.Windows;
using System.Windows.Controls;

using YouAre.MVVM.Model;

namespace YouAre.MVVM.View
{
    public partial class Login : UserControl
    {
        private readonly Frame _navFrame;
        private readonly Server _server;

        public Login(Frame navFrame, Server server)
        {
            InitializeComponent();
            _navFrame = navFrame;
            _server = server;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var account = await _server.Login(txtUsername.Text, txtPassword.Password);
            if (account != Account.Empty)
            {
                var app = new ApplicationYouAre(_navFrame, account);
                _navFrame.Navigate(app);
            }
            else
            {
                MessageBox.Show("There is no such account!");
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var register = new Register(_navFrame, _server);
            _navFrame.Navigate(register);
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}