using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static MainDesign.RegPage;

namespace MainDesign
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Викликаємо метод для ініціалізації
            Initialize();
        }

        private async void Initialize()
        {
            await Task.Delay(500);

            Application.Current.Dispatcher.Invoke(() =>
            {

                DoubleAnimation growAnimation = new DoubleAnimation
                {
                    From = 80,
                    To = 170,  // Збільшуємо розмір тексту
                    Duration = TimeSpan.FromSeconds(0.75)
                };

                textBlock.BeginAnimation(TextBlock.FontSizeProperty, growAnimation);

                
            });

            await Task.Delay(3000);

            var loginPage = new LoginPage();
            loginPage.LoginRequested += LoginRequestedHandler;

            //mainFrame.Visibility = Visibility.Collapsed;

            mainFrame.Navigate(loginPage);
        }

        private void LoginRequestedHandler(object sender, EventArgs e)
        {
            while (mainFrame.NavigationService.CanGoBack)
            {
                mainFrame.NavigationService.RemoveBackEntry();
            }

            if (e is EventArgsWithRegistrationPage args && args.OpenRegistrationPage)
            {
                mainFrame.Navigate(new RegPage());
            }
            else if (e is EventArgsWithLoginPage args1 && args1.OpenLoginPage)
            {
                mainFrame.Navigate(new LoginPage());
            }
            else
            {
                mainFrame.Navigate(new CommPage());
            }
        }

        //private void LoginButton_Click(object sender, RoutedEventArgs e)
        //{
        //    mainFrame.Navigate(new CommPage());
        //}

        //private void RegisterButton_Click(object sender, RoutedEventArgs e)
        //{
        //    mainFrame.Navigate(new RegPage());
        //}
        private void AlreadyRegisteredButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new LoginPage());
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
