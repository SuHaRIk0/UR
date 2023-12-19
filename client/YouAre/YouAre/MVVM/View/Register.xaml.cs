using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using YouAre.MVVM.Model;


using YouAre.MVVM.Model;


namespace YouAre.MVVM.View
{
    public partial class Register
    {
        private const string ApiBaseUrl = "http://localhost:5131/api/user";

        private readonly Frame _navFrame;
        private readonly Server _server;

        public Register(Frame navFrame, Server server)
        {
            InitializeComponent();
            _navFrame = navFrame;
            _server = server;
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var user = new User
            {
                Username = txtUsername.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Password
            };

            await SendRequest("register", user);
        }

        private void ToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            var login = new Login(_navFrame, _server);
            _navFrame.Navigate(login);
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async Task SendRequest(string endpoint, object data)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var response = await httpClient.PostAsync($"{ApiBaseUrl}/{endpoint}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Request successful!");
                    }
                    else
                    {
                        MessageBox.Show($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
}

