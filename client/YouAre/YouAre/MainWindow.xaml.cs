using MaterialDesignThemes.Wpf;

using System.Windows;
using System.Windows.Input;

using YouAre.MVVM.Model;
using YouAre.MVVM.View;

namespace YouAre
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Server _server;
        private readonly string _url = "http://localhost:5131";

        public MainWindow()
        {
            InitializeComponent();
            _server = new Server(_url);
            var login = new Login(_NavigationFrame, _server);

            _NavigationFrame.Navigate(login);
        }

        public bool IsDarckTheme { get; set; }

        private readonly PaletteHelper paletteHelper = new PaletteHelper();

        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();
            if(IsDarckTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarckTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarckTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }


    }
}