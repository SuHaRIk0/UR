using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace MainDesign
{
    public partial class RegPage : Page
    {
        public event EventHandler RegisterRequested;

        private const string BaseUrl = "http://localhost:5131";

        public RegPage()
        {
            InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string apiUrl = $"{BaseUrl}/register"; 
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
                        RegisterRequested?.Invoke(this, new EventArgsWithLoginPage());
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
            RegisterRequested?.Invoke(this, new EventArgsWithLoginPage());
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        public class EventArgsWithLoginPage : EventArgs
        {
            public bool OpenRegistrationPage { get; set; } = false;
            public bool OpenLoginPage { get; set; } = true;
        }
    }
}