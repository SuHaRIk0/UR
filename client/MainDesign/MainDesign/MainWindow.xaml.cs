using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MainDesign
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Task.Delay(3000).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DoubleAnimation animation = new DoubleAnimation
                    {
                        From = 100,
                        To = 170,
                        Duration = TimeSpan.FromSeconds(1.5)
                    };

                    textBlock.BeginAnimation(TextBlock.FontSizeProperty, animation);

                    var loginPage = new LoginPage();
                    loginPage.LoginRequested += LoginRequestedHandler;
                    mainFrame.Navigate(loginPage);
                });
            });
        }

        private void NavigateToLoginPage()
        {
            var loginPage = new LoginPage();
            loginPage.LoginRequested += LoginRequestedHandler;
            mainFrame.Navigate(loginPage);
        }

        private void LoginRequestedHandler(object sender, EventArgs e)
        {
            if (e is EventArgsWithRegistrationPage args && args.OpenRegistrationPage)
            {
                mainFrame.Navigate(new RegPage());
            }
            else
            {
                // Логіка для обробки інших випадків
                NavigateToCommPage();
            }
        }

        private void NavigateToCommPage()
        {
            mainFrame.Navigate(new CommPage());
        }

        // Додайте обробник кнопки для логін пейджа
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToCommPage();
        }

        // Додайте обробник кнопки для регістрації
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new RegPage());
        }

        // Додайте обробник кнопки для переключення на логін пейдж
        private void AlreadyRegisteredButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToLoginPage();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
