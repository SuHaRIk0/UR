using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using YouAre.MVVM.Model;

namespace YouAre.MVVM.View.Subviews
{
    public partial class Posts : UserControl
    {
        public Posts()
        {
            InitializeComponent();
            InitializeDataAsync_1();
        }

        private async void InitializeDataAsync_1()
        {
            var server = new Server("http://localhost:5131");
            var posts = await server.GetPostsAsync();
            lvDataBinding.ItemsSource = posts;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var createPostPopup = new CreatePostPopup();

            createPostPopup.DataContext = DataContext;

            Window popupWindow = new Window
            {
                Content = createPostPopup,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStyle = WindowStyle.ToolWindow,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            bool? result = popupWindow.ShowDialog();

            if (result.HasValue && result.Value)
            {
                InitializeDataAsync_1();
            }
        }

        private void Card_LayoutUpdated(object sender, EventArgs e)
        {

        }

        private void lvDataBinding_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }


    }
}
