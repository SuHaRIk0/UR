using System.Windows.Controls;
using YouAre.MVVM.Model;
using YouAre.MVVM.ViewModel.SubViewModel;
using System.Windows;
using System;

namespace YouAre.MVVM.View.Subviews
{
    public partial class EditAvaPopup : UserControl
    {
        public EditAvaPopup()
        {
            InitializeComponent();
            DataContext = new EditAvaPopupViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InitializeDataAsync_7();
        }

        private async void InitializeDataAsync_7()
        {
            var server = new Server("http://localhost:5131");

            var newAva = new UsersProfile
            {
                ProfilePhoto = (DataContext as EditAvaPopupViewModel)?.NewPostImagePath ?? ""
            };

            var userProfile = await server.EditAvaAsync(newAva);

            // Set the NewPostImagePath property in the EditAvaPopupViewModel
            //(DataContext as EditAvaPopupViewModel)?.OnPropertyChanged(userProfile.ProfilePhoto);
        }
    }
}
