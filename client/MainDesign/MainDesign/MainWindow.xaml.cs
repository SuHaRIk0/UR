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

        private void NavigateToLoginPage()
        {
            // Створюємо і показуємо логін сторінку
            var loginPage = new LoginPage();
            loginPage.LoginRequested += LoginRequestedHandler;

            // Ховаємо головну сторінку
            mainFrame.Visibility = Visibility.Collapsed;

            // Навігація на логін сторінку
            mainFrame.Navigate(loginPage);
        }

        private void LoginRequestedHandler(object sender, EventArgs e)
        {
            if (e is EventArgsWithRegistrationPage args && args.OpenRegistrationPage)
            {
                while (mainFrame.NavigationService.CanGoBack)
                {
                    mainFrame.NavigationService.RemoveBackEntry();
                }

                // Створюємо і показуємо реєстраційну сторінку
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
            // Створюємо і показуємо сторінку "CommPage"
            var commPage = new CommPage();

            // Ховаємо головну сторінку
            mainFrame.Visibility = Visibility.Collapsed;

            // Навігація на "CommPage"
            mainFrame.Navigate(commPage);
        }

        // Додайте обробник кнопки для логін пейджа
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToCommPage();
        }

        // Додайте обробник кнопки для реєстрації
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Створюємо і показуємо реєстраційну сторінку
            mainFrame.Navigate(new RegPage());
        }

        // Додайте обробник кнопки для переходу на логін пейдж
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
