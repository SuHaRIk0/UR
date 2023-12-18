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
using YouAre.MVVM.ViewModel;
using YouAre.MVVM.ViewModel.SubViewModel;

namespace YouAre.MVVM.View.Subviews
{
    public partial class Users : UserControl
    {
        public Users()
        {
            InitializeComponent();
            InitializeDataAsync();
        }

        private async void InitializeDataAsync()
        {
            var server = new Server("http://localhost:5131");
            var users = await server.GetUsersAsync();
            lvDataBinding.ItemsSource = users;
        }


        private void Card_LayoutUpdated(object sender, EventArgs e)
        {
            
        }

        private void lvDataBinding_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
