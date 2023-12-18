using YouAre.MVVM.Model;

namespace YouAre.MVVM.ViewModel.SubViewModel
{
    class UsersSubViewModel
    {
        public readonly Server _server;

        public UsersSubViewModel(Server server)
        {
            _server = server;
        }

    }
}
