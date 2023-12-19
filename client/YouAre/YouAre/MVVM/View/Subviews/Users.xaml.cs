//using LiveCharts.Wpf;
//using MaterialDesignThemes.Wpf;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Linq;
//using YouAre.MVVM.Model;
//using System.Diagnostics;

//namespace YouAre.MVVM.View.Subviews
//{
//    public partial class Users : UserControl, INotifyPropertyChanged
//    {
//        private List<DisplayMassage> messages = new List<DisplayMassage>();
//        private readonly UsersProfile prof;

//        public event PropertyChangedEventHandler PropertyChanged;

//        public List<DisplayMassage> Messages
//        {
//            get { return messages; }
//            set
//            {
//                messages = value;
//                OnPropertyChanged(nameof(Messages));
//            }
//        }

//        public Users()
//        {
//            InitializeComponent();
//            DataContext = this;
//            InitializeDataAsync();
//            prof = new UsersProfile()
//            {
//                Username = "Ri",
//                Id = 2,
//                Description = "An exuberant troublemaker with an exceptional talent for trickery"
//            };
//        }

//        private async void InitializeDataAsync()
//        {
//            var server = new Server("http://localhost:5131");
//            var users = await server.GetUsersAsync();

//            if (users != null)
//            {
//                lvDataBinding.ItemsSource = users;
//            }
//        }

//        private async Task HandleButtonClickAsync()
//        {
//            var server = new Server("http://localhost:5131");

//            lvChatPage.Visibility = Visibility.Visible;
//            lvHeaderText.Text = "Direct Chat with";

//            if (lvDataBinding.SelectedIndex >= 0)
//            {
//                if (lvDataBinding.SelectedItem is UsersProfile selectedItem)
//                {
//                    lvUsername.Text = selectedItem.Username;

//                    Trace.WriteLine($"non active cycle");

//                    var chat = await server.GetChatAsync(selectedItem.Id);


//                    lvMessageBinding.ItemsSource = Messages;

//                    lvMessageBinding.Items.Refresh();

//                    if (chat != null && chat.Messages != null)
//                    {
//                        // Відлагодження: Вивести кількість повідомлень, які приходять з сервера
//                        Trace.WriteLine($"Received {chat.Messages.Count} messages from the server.");

//                        // Convert server messages to your DisplayMassage model
//                        var serverMessages = chat.Messages.Select(msg => new DisplayMassage
//                        {
//                            Author = msg.AuthorId.ToString(),
//                            Text = msg.Text,
//                            SentAt = msg.SentAt
//                        });

//                        // Відлагодження: Вивести кількість повідомлень, які будуть встановлені в Messages
//                        Trace.WriteLine($"Setting Messages to {serverMessages.Count()} messages.");

//                        Messages = new List<DisplayMassage>(serverMessages);

//                        // Відлагодження: Вивести кількість повідомлень у властивості Messages
//                        Trace.WriteLine($"Messages now contains {Messages.Count} messages.");

//                        lvMessageBinding.ItemsSource = Messages;

//                        lvMessageBinding.Items.Refresh();

//                    }
//                }
//            }
//        }


//        private void Button_ClickAsync(object sender, RoutedEventArgs e)
//        {
//            _ = HandleButtonClickAsync();
//        }

//        private void OnPropertyChanged(string propertyName)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }

//        private async void Button_Click_1(object sender, RoutedEventArgs e)
//        {
//            if (lvTextBox.Text != null)
//            {
//                if (lvDataBinding.SelectedItem is UsersProfile selectedItem)
//                {
//                    var messageModel = new Message
//                    {
//                        RecipientId = selectedItem.Id,
//                        Text = lvTextBox.Text,
//                    };

//                    var server = new Server("http://localhost:5131");
//                    var response = await server.PostMessageAsync(messageModel);

//                    if (response != null)
//                    {
//                        Messages.Add(new DisplayMassage
//                        {
//                            Author = prof.Username,
//                            Text = lvTextBox.Text,
//                            SentAt = DateTime.Now
//                        });

//                        lvMessageBinding.Items.Refresh();

//                        lvTextBox.Clear();
//                    }
//                }
//            }
//        }

//        private void Card_LayoutUpdated(object sender, EventArgs e)
//        {

//        }
//    }
//}


using MaterialDesignThemes.Wpf;

using System.Windows;
using System.Windows.Controls;

using YouAre.MVVM.Model;

namespace YouAre.MVVM.View.Subviews
{
    public partial class Users : UserControl
    {
        private List<DisplayMassage> messages = new List<DisplayMassage>();
        private readonly UsersProfile prof;


        public Users()
        {
            InitializeComponent();
            InitializeDataAsync();
        }

        private async void InitializeDataAsync()
        {
            var server = new Server("http://localhost:5131");
            var users = await server.GetUsersAsync();

            lvDataBinding.ItemsSource = users;
        }


        private void Card_LayoutUpdated(object sender, EventArgs e)
        {

        }

        private void lvDataBinding_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }

        //private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    lvChatPage.Visibility = Visibility.Visible;
        //    lvHeaderText.Text = "Direct Chat with";

        //    if (lvDataBinding.SelectedIndex >= 0)
        //    {
        //        if (lvDataBinding.SelectedItem is UsersProfile selectedItem)
        //        {
        //            lvUsername.Text = selectedItem.Username;
        //            messages.Add(new()
        //            {
        //                Author = selectedItem.Username,
        //                Text = "Firs message...",
        //                SentAt = DateTime.Now
        //            });
        //            messages.Add(new()
        //            {
        //                Author = selectedItem.Username,
        //                SentAt = DateTime.Now,
        //                Text = "Another message."
        //            });
        //            lvMessageBinding.ItemsSource = messages;
        //        }
        //    }
        //}

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lvChatPage.Visibility = Visibility.Visible;
                lvHeaderText.Text = "Direct Chat with";

                if (lvDataBinding.SelectedIndex >= 0)
                {
                    if (lvDataBinding.SelectedItem is UsersProfile selectedItem)
                    {
                        var server = new Server("http://localhost:5131");
                        var chatMessages = await server.GetChatAsync(selectedItem.Username);

                        if (chatMessages != null)
                        {
                            messages = chatMessages.Select(msg => new DisplayMassage
                            {
                                Author = msg.AuthorId == selectedItem.Id ? selectedItem.Username : "Other User",
                                Text = msg.Text,
                                SentAt = msg.SentAt
                            }).ToList();

                            lvMessageBinding.ItemsSource = messages;

                            messages.Add(new DisplayMassage
                            {
                                Author = prof?.Username,
                                Text = lvTextBox.Text,
                                SentAt = DateTime.Now
                            });

                            lvMessageBinding.Items.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lvTextBox.Text != null && lvDataBinding.SelectedItem is UsersProfile selectedItem)
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
                        // Assuming 'prof' is an instance of UsersProfile, adjust as needed
                        messages.Add(new DisplayMassage
                        {
                            Author = prof?.Username,
                            Text = lvTextBox.Text,
                            SentAt = DateTime.Now
                        });

                        lvMessageBinding.Items.Refresh();
                        lvTextBox.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}