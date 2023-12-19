//using MaterialDesignThemes.Wpf;
//using System.Windows;
//using System.Windows.Controls;
//using YouAre.MVVM.Model;

//namespace YouAre.MVVM.View.Subviews
//{
//    public partial class Posts : UserControl
//    {
//        public Posts()
//        {
//            InitializeComponent();
//            InitializeDataAsync_1();
//        }

//        private async void InitializeDataAsync_1()
//        {
//            var server = new Server("http://localhost:5131");
//            var posts = await server.GetPostsAsync();
//            lvDataBinding.ItemsSource = posts;
//        }

//        private async void Button_Click(object sender, RoutedEventArgs e)
//        {
//            var createPostPopup = new CreatePostPopup();

//            createPostPopup.DataContext = DataContext;

//            Window popupWindow = new Window
//            {
//                Content = createPostPopup,
//                SizeToContent = SizeToContent.WidthAndHeight,
//                WindowStyle = WindowStyle.ToolWindow,
//                ResizeMode = ResizeMode.NoResize,
//                WindowStartupLocation = WindowStartupLocation.CenterScreen
//            };

//            bool? result = popupWindow.ShowDialog();

//            if (result.HasValue && result.Value)
//            {
//                InitializeDataAsync_1();
//            }
//        }

//        private void Card_LayoutUpdated(object sender, EventArgs e)
//        {

//        }

//        private void lvDataBinding_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
//        {

//        }


//    }
//}


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

            var posts = new List<Publication>()
            {
                new()
                {
                    Text = "Some cool stuff going around...",
                    PostAt = DateTime.Now,
                    Picture = "https://t4.ftcdn.net/jpg/01/04/78/75/360_F_104787586_63vz1PkylLEfSfZ08dqTnqJqlqdq0eXx.jpg",
                    
                },
                new()
                {
                    Text = "Погляньте на цих курдупликів!!!",
                    PostAt = DateTime.Now,
                    Picture = "https://wallpapers.com/images/hd/funny-cats-pictures-uu9qufqc5zq8l7el.jpg",
                }
            };

            lvDataBinding.ItemsSource = posts;
        }

        private async void InitializeDataAsync_1()
        {
            var server = new Server("http://localhost:5131");
            //var posts = await server.GetPostsAsync();
            //vDataBinding.ItemsSource = posts;
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

            lvDataBinding.Items.Refresh();
        }

        private void Card_LayoutUpdated(object sender, EventArgs e)
        {

        }

        private void lvDataBinding_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}