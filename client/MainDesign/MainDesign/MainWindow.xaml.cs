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
                    To = 170,  
                    Duration = TimeSpan.FromSeconds(0.75)
                };

                textBlock.BeginAnimation(TextBlock.FontSizeProperty, growAnimation);

                
            });

            await Task.Delay(3000);

            var loginPage = new LoginPage();
            loginPage.LoginRequested += LoginRequestedHandler;

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
                var RegPage = new RegPage();
                RegPage.RegisterRequested += LoginRequestedHandler;
                mainFrame.Navigate(RegPage);
            }
            else if (e is EventArgsWithLoginPage args1 && args1.OpenLoginPage)
            {
                var loginPage = new LoginPage();
                loginPage.LoginRequested += LoginRequestedHandler;
                mainFrame.Navigate(loginPage);
            }
            else if (e is EventArgsWitCommPage args2 && args2.OpenCommPage)
            {
                var CommPage = new CommPage();
                CommPage.CommRequested += LoginRequestedHandler;
                mainFrame.Navigate(CommPage);
            }
            else if (e is EventArgsWitMainPage args3 && args3.OpenMainMenuPage)
            {
                var MainMenu = new MainMenu();
                MainMenu.MainMenuRequested += LoginRequestedHandler;
                mainFrame.Navigate(MainMenu);
            }
            else if (e is EventArgsUserProfilPage args4 && args4.OpenUserWiget)
            {
                var UserWiget = new UserWiget();
                UserWiget.UserWigetRequested += LoginRequestedHandler;
                mainFrame.Navigate(UserWiget);
            }
            else if (e is EventArgsEditUserProfilPage args5 && args5.OpenEditUserWiget)
            {
                var UserWiget = new UserWiget();
                UserWiget.UserWigetRequested += LoginRequestedHandler;
                mainFrame.Navigate(UserWiget);
                var UserEditWiget = new UserEditWiget();
                UserEditWiget.EditUserRequested += LoginRequestedHandler;
                mainFrame.Navigate(UserEditWiget);
            }
        }
        

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new CommPage());
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new RegPage());
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new LoginPage());
        }

        private void AlreadyRegisteredButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new LoginPage());
        }
        private void CommEnter_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new MainMenu());
        }
        private void UserProfil_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new UserWiget());
        }
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new UserWiget());
            mainFrame.Navigate(new UserEditWiget());
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
