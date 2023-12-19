using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using YouAre.MVVM.Model;

namespace YouAre.MVVM.View.Subviews
{
    public partial class Users : UserControl, INotifyPropertyChanged
    {
        private List<DisplayMassage> messages = new List<DisplayMassage>();
        private readonly UsersProfile prof;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<DisplayMassage> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }

        public Users()
        {
            InitializeComponent();
            InitializeDataAsync();
        }

        private async void InitializeDataAsync()
        {
            var server = new Server("http://localhost:5131");
            var users = await server.GetUsersAsync();

            if (users != null)
            {
                lvDataBinding.ItemsSource = users;
            }
        }

        private async Task HandleButtonClickAsync()
        {
            var server = new Server("http://localhost:5131");

            lvChatPage.Visibility = Visibility.Visible;
            lvHeaderText.Text = "Direct Chat with";

            if (lvDataBinding.SelectedIndex >= 0)
            {
                if (lvDataBinding.SelectedItem is UsersProfile selectedItem)
                {
                    lvUsername.Text = selectedItem.Username;

                    // Fetch messages from the server
                    var chat = await server.GetChatAsync(selectedItem.Id);

                    if (chat != null && chat.Messages != null)
                    {
                        // Convert server messages to your DisplayMassage model
                        var serverMessages = chat.Messages.Select(msg => new DisplayMassage
                        {
                            Author = msg.AuthorId.ToString(),
                            Text = msg.Text,
                            SentAt = msg.SentAt
                        });

                        // Add the server messages to the local messages list
                        Messages = new List<DisplayMassage>(serverMessages);
                    }
                }
            }
        }

        private void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            _ = HandleButtonClickAsync();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (lvTextBox.Text != null)
            {
                if (lvDataBinding.SelectedItem is UsersProfile selectedItem)
                {
                    var messageModel = new Message
                    {
                        RecipientId = selectedItem.Id,
                        Text = lvTextBox.Text,
                    };

                    var server = new Server("http://localhost:5131");
                    var response = await server.PostMessageAsync(messageModel);

                    if (response != null)
                    {
                        Messages.Add(new DisplayMassage
                        {
                            Author = prof.Username,
                            Text = lvTextBox.Text,
                            SentAt = DateTime.Now
                        });

                        lvMessageBinding.Items.Refresh();

                        lvTextBox.Clear();
                    }
                }
            }
        }

        private void Card_LayoutUpdated(object sender, EventArgs e)
        {

        }
    }
}
