using System.Collections.ObjectModel;
using System.Windows;

namespace MainDesign
{
    public class Post
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
    }

    public partial class PostWidget : Window
    {
        public ObservableCollection<Post> Posts { get; set; } = new ObservableCollection<Post>();

        public PostWidget()
        {
            InitializeComponent();
            LoadPostsFromDatabase();
            DataContext = this;
        }

        private void LoadPostsFromDatabase()
        {
            Posts.Add(new Post { Title = "Post 1", Content = "Content 1", ImageUrl = "path/to/image1.jpg" });
            Posts.Add(new Post { Title = "Post 2", Content = "Content 2", ImageUrl = "path/to/image2.jpg" });
        }
    }
}
