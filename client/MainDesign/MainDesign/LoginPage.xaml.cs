using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace MainDesign
{
    public partial class LoadinPage : Window
    {

        public LoadinPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string apiUrl = "http://localhost:5131/register";
                    string email = emailTextBox.Text;
                    string password = passwordTextBox.Text;

                    var registrationData = new
                    {
                        email,
                        password
                    };

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(registrationData), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Registration successful!");
                        // Navigate to CommPage
                        var commPageWindow = new CommPage();
                        commPageWindow.Show();
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Registration failed: {errorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception during registration: {ex.Message}");
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
