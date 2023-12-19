// CreatePostPopup.xaml.cs

using System.Windows.Controls;
using YouAre.MVVM.Model;
using YouAre.MVVM.ViewModel.SubViewModel;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System;

namespace YouAre.MVVM.View.Subviews
{
    public partial class CreatePostPopup : UserControl
    {
        public CreatePostPopup()
        {
            InitializeComponent();
            DataContext = new CreatePostPopupViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InitializeDataAsync_4();
        }

        private async void InitializeDataAsync_4()
        {
            var server = new Server("http://localhost:5131");

            var newPost = new Publication
            {
                Text = (DataContext as CreatePostPopupViewModel)?.NewPostTitle ?? "", 
                Picture = (DataContext as CreatePostPopupViewModel)?.NewPostImagePath ?? "" 
            };

            var userProfile = await server.PostPostAsync(newPost);
        }

    }
}
