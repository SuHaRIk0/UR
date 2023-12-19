using System.ComponentModel;
using YouAre.MVVM.Model;
using YouAre.MVVM.View.Subviews;

public class CreatePostPopupViewModel : Posts, INotifyPropertyChanged
{
    private Publication _newPost;

    public CreatePostPopupViewModel()
    {
        _newPost = new Publication();
    }

    public string NewPostTitle
    {
        get { return _newPost.Text; }
        set
        {
            if (_newPost.Text != value)
            {
                _newPost.Text = value;
                OnPropertyChanged(nameof(NewPostTitle));
            }
        }
    }

    public string NewPostImagePath
    {
        get { return _newPost.Picture; }
        set
        {
            if (_newPost.Picture != value)
            {
                _newPost.Picture = value;
                OnPropertyChanged(nameof(NewPostImagePath));
            }
        }
    }

    // Додайте інші властивості, якщо потрібно

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public Publication GetNewPost()
    {
        return _newPost;
    }
}
