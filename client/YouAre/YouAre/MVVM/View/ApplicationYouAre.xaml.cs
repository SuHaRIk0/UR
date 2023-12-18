using System.Windows;
using System.Windows.Controls;

using YouAre.MVVM.Model;

namespace YouAre.MVVM.View
{
    public partial class ApplicationYouAre : UserControl
    {
        private readonly Frame _navFrame;
        private readonly Account _account;
        public ApplicationYouAre(Frame navFrame, Account account)
        {
            InitializeComponent();
            _navFrame = navFrame;
            _account = account;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
