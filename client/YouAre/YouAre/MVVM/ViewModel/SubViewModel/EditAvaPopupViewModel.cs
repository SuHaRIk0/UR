using System.ComponentModel;
using YouAre.MVVM.Model;
using YouAre.MVVM.View.Subviews;

namespace YouAre.MVVM.ViewModel.SubViewModel
{
    public class EditAvaPopupViewModel : UsersProfile, INotifyPropertyChanged
    {
        private string _newPostTitle;
        public string NewPostTitle
        {
            get { return _newPostTitle; }
            set
            {
                if (_newPostTitle != value)
                {
                    _newPostTitle = value;
                    OnPropertyChanged(nameof(NewPostTitle));
                }
            }
        }

        private string _newPostImagePath;
        public string NewPostImagePath
        {
            get { return _newPostImagePath; }
            set
            {
                if (_newPostImagePath != value)
                {
                    _newPostImagePath = value;
                    OnPropertyChanged(nameof(NewPostImagePath));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}