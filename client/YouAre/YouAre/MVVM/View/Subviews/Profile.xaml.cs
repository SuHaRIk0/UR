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
    public partial class Profile : UserControl
    {
        public Profile()
        {
            InitializeComponent();
            InitializeDataAsync_3();
        }

        private async void InitializeDataAsync_3()
        {
            var server = new Server("http://localhost:5131");
            var users = await server.GetUserAsync();
            lvDataBinding.ItemsSource = users;
        }


        private void Card_LayoutUpdated(object sender, EventArgs e)
        {

        }

        private void lvDataBinding_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
        private void EditToggle_Checked(object sender, RoutedEventArgs e)
        {
            // Implementation for EditToggle_Checked event...
        }

        private void EditToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            // Implementation for EditToggle_Unchecked event...
        }
    }
}
