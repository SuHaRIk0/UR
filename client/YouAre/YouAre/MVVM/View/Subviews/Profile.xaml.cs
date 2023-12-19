using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using YouAre.MVVM.Model;
using YouAre.MVVM.ViewModel.SubViewModel;

namespace YouAre.MVVM.View.Subviews
{
    public partial class Profile : UserControl
    {
        private readonly Server _server;

        public Profile()
        {
            InitializeComponent();
            _server = new Server("http://localhost:5131");
            InitializeDataAsync();
        }

        private async void InitializeDataAsync()
        {
            var userProfile = await _server.GetUserAsync();

            if (userProfile != UsersProfile.Empty)
            {
                DisplayUserProfile(userProfile);
            }
        }

        private void DisplayUserProfile(UsersProfile userProfile)
        {
            lvID.Text = $"ID: {userProfile.Id}";
            lvUsername.Text = userProfile.Username;
            lvProfileImage.Source = new BitmapImage(new Uri(userProfile.ProfilePhoto));
            lvDescription.Text = userProfile.Description;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            InitializeDataAsync_5();
        }

        private async void InitializeDataAsync_5()
        {
            var server = new Server("http://localhost:5131");

            var newPost = new UsersProfile
            {
                ProfilePhoto = (DataContext as CreatePostPopupViewModel)?.NewPostImagePath ?? ""
            };

            var userProfile = await server.EditAvaAsync(newPost);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            InitializeDataAsync_6();
        }

        private async void InitializeDataAsync_6()
        {
            var server = new Server("http://localhost:5131");

            var newPost = new UsersProfile
            {
                Description = (DataContext as CreatePostPopupViewModel)?.NewPostImagePath ?? ""
            };

            var userProfile = await server.EditAvaAsync(newPost);
        }
        private void Card_LayoutUpdated(object sender, EventArgs e)
        {

        }
    }
}
