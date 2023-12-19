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

        private async void Button_Click_start(object sender, RoutedEventArgs e)
        { 
            var server = new Server("http://localhost:5131");

            var viewModel = (DataContext as CreatePostPopupViewModel);

            var newPost = new Publication
            {
                Text = viewModel?.NewPostTitle ?? "",
                Picture = viewModel?.NewPostImagePath ?? ""
            };

            var userProfile = await server.PostPostAsync(newPost);
        }
    }
}