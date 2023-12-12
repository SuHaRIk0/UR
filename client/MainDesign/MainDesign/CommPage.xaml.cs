using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace MainDesign
{
    public partial class CommPage : Page
    {
        public event EventHandler CommRequested;

        private const string ApiBaseUrl = "http://localhost:5131/api/";

        public CommPage()
        {
            InitializeComponent();
            //LoadCommunitiesAsync();
        }

        private async Task LoadCommunitiesAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync($"{ApiBaseUrl}communities");
                    var communities = JsonConvert.DeserializeObject<List<Community>>(response);

                    for (int i = 0; i < communities.Count && i < 4; i++)
                    {
                        string profilePhotoUrl = communities[i].CommunityProfilePhoto;

                        var imageControl = FindName($"Image{i}") as System.Windows.Controls.Image;

                        if (imageControl != null)
                        {
                            //imageControl.Source = await LoadImageAsync(profilePhotoUrl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading communities: {ex.Message}");
            }


        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CommEnter_Click(object sender, RoutedEventArgs e)
        {
            CommRequested?.Invoke(this, new EventArgsWitMainPage());

        }

        private class Community
        {
            public int Id { get; set; }
            public int AuthorId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public List<int> Members { get; set; }
            public string CommunityProfilePhoto { get; set; }
        }
    }

    public class EventArgsWitMainPage : EventArgs
    {
        public bool OpenMainMenuPage { get; set; } = true;
        public bool OpenCommPage { get; set; } = false;
    }
}
