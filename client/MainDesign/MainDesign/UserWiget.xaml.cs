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

namespace MainDesign
{
    public partial class UserWiget : Page
    {
        public event EventHandler UserWigetRequested;
        public UserWiget()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            UserWigetRequested?.Invoke(this, new EventArgsEditUserProfilPage());
        }
    }

    public class EventArgsEditUserProfilPage : EventArgs
    {
        public bool OpenEditUserWiget { get; set; } = true;
        public bool OpenUserWiget { get; set; } = true;
    }
}
