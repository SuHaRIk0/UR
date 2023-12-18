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
    public partial class Chats : UserControl
    {
        public Chats()
        {
            InitializeComponent();
            InitializeDataAsync_2();
        }

        private async void InitializeDataAsync_2()
        {
            var server = new Server("http://localhost:5131");
            var chats = await server.GetChatsAsync();
            lvDataBinding.ItemsSource = chats;
        }

        private void Card_LayoutUpdated(object sender, EventArgs e)
        {

        }

        private void lvDataBinding_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}

