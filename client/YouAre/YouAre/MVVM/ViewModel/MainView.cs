using YouAre.Core;
using YouAre.MVVM.Model;
using YouAre.MVVM.ViewModel.SubViewModel;

namespace YouAre.MVVM.ViewModel
{
    class MainView : ObservableObject
    {
        // SubCommands.
        public RelayCommand ProfilesCommand { get; set; }
        public RelayCommand ChatsCommand { get; set; }
        public RelayCommand UsersCommand { get; set; }
        public RelayCommand PostsCommand { get; set; }
       
        // Subviews.
        public UsersSubViewModel UsersSubV { get; set; }
        public PostsSubViewModel PostsSubV {  get; set; }
        public ChatsSubViewModel ChatsSubV {  get; set; }
        public ProfileSubViewModel ProfileSubV {  get; set; }

        private object _currentSubview;

        public object CurrentSubview
        {
            get { return _currentSubview; }
            set
            { 
                _currentSubview = value;
                OnPropertyChanged();
            }
        }

        private readonly Server _server;

        public MainView()
        {
            _server = new Server("http://localhost:5131");
            UsersSubV = new UsersSubViewModel(_server);
            PostsSubV = new PostsSubViewModel();
            ChatsSubV = new ChatsSubViewModel();
            ProfileSubV = new ProfileSubViewModel();
            CurrentSubview = PostsSubV;

            UsersCommand = new RelayCommand(o =>
            {
                CurrentSubview = UsersSubV;
            });

            PostsCommand = new RelayCommand(o =>
            {
                CurrentSubview = PostsSubV;
            });

            ChatsCommand = new RelayCommand(o =>
            {
                CurrentSubview = ChatsSubV;
            });

            ProfilesCommand = new RelayCommand(o =>
            {
                CurrentSubview = ProfileSubV;
            });
        }
    }
}
