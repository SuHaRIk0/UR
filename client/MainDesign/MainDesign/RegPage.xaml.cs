using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace MainDesign
{
    public partial class RegRage : Window
    {
        public event EventHandler LoginRequested;

        private const string BaseUrl = "http://localhost:5131";

        public RegRage()
        {
            InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string apiUrl = $"{BaseUrl}/api/register";
                    string email = emailTextBox.Text;
                    string password = passwordTextBox.Text;
                    var registrationData = new
                    {
                        Email = email,
                        Password = password
                    };
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(registrationData), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string successMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Registration successful: {successMessage}");

                        // Raise the LoginRequested event
                        LoginRequested?.Invoke(this, EventArgs.Empty);
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

        private void AlreadyRegisteredButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise the LoginRequested event
            LoginRequested?.Invoke(this, EventArgs.Empty);
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
