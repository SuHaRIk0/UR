using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private void Card_LayoutUpdated(object sender, EventArgs e)
        {

        }

        private void lvDataBinding_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}