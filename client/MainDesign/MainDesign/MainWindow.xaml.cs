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

                    this.Hide();

                    var loginWindow = new LoadinPage();
                    loginWindow.Show();
                });
            });
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
