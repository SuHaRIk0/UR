using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
//using System.Text.Json;
//using System.Text.Json.Serialization;




namespace YouAre.MVVM.Model
{
    public class Server
    {
        private readonly string _baseApiUrl;
        private static string _accessToken;
        private static int _userId;


        public Server(string baseApiUrl)
        {
            _baseApiUrl = baseApiUrl;
        }
        //=====================================================================================================================
        //                                              A   C   C   O   U   N   T   S
        //=====================================================================================================================
        public async Task<Account> Login(string username, string password)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };

            User user = new User()
            {
                Password = password,
                Username = username,
                Email = username
            };

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/User/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(responseContent))
                {
                    var responseData = JsonConvert.DeserializeObject<Account>(responseContent);

                    if (responseData != null)
                    {
                        _accessToken = responseData.Token;
                        _userId = responseData.UserId;
                        return new Account();
                    }
                }
            }

            return null;
        }



        //=====================================================================================================================
        //                                              C   H   A   T   S
        //=====================================================================================================================
        public async Task<List<UsersProfile>> GetChatsAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    MessageBox.Show("Будь ласка, увійдіть в систему.");
                    return new List<UsersProfile>();
                }

                using HttpClient client = new()
                {
                    BaseAddress = new Uri(_baseApiUrl)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await client.GetAsync($"api/user/chats/{_userId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    try
                    {
                        List<UsersProfile> responseData = JsonConvert.DeserializeObject<List<UsersProfile>>(responseContent);

                        if (responseData != null)
                        {
                            return responseData;
                        }
                        else
                        {
                            MessageBox.Show("Помилка: пуста відповідь від сервера.");
                            return new List<UsersProfile>();
                        }
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show($"Помилка при десеріалізації відповіді сервера: {ex.Message}");
                        return new List<UsersProfile>();
                    }
                }
                else
                {
                    MessageBox.Show($"Помилка при отриманні відповіді від сервера: {response.StatusCode}");
                    return new List<UsersProfile>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
                return new List<UsersProfile>();
            }
        }


        public async Task<Chat> GetChatAsync(int id)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };
            var response = await client.GetFromJsonAsync<Chat>($"chats/{id}/");
            return response;
        }

        public async Task<string?> PostChatAsync(Chat model)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };
            var response = await client.PostAsJsonAsync("chats", model);
            return response.IsSuccessStatusCode ? response.Content.ToString() : " ";
        }

        public async Task<string?> PutChatAsync(Chat model)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };
            var response = await client.PutAsJsonAsync("chats", model);
            return response.IsSuccessStatusCode ? response.Content.ToString() : " ";
        }

        public async Task<string?> DeleteChatAsync(int id)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };
            var response = await client.DeleteAsync($"chats/{id}");
            return response.IsSuccessStatusCode ? response.Content.ToString() : " ";
        }
        //=====================================================================================================================
        //                                          M  E  S  S   A   G   E   S
        //=====================================================================================================================
        public async Task<List<Message>> GetMessagesAsync()
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };
            var responce = await client.GetFromJsonAsync<List<Message>>($"messages/");
            return responce ?? [];
        }

        public async Task<Message> GetMessageAsync(int id)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };
            var response = await client.GetFromJsonAsync<Message>($"messages/{id}/");
            return response;
        }

        public async Task<string?> PostMessageAsync(Message model)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };
            var response = await client.PostAsJsonAsync("messages", model);
            return response.IsSuccessStatusCode ? response.Content.ToString() : " ";
        }

        public async Task<string?> PutMessageAsync(Message model)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };
            var response = await client.PutAsJsonAsync("messages", model);
            return response.IsSuccessStatusCode ? response.Content.ToString() : " ";
        }

        public async Task<string?> DeleteMessageAsync(int id)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };
            var response = await client.DeleteAsync($"messages/{id}");
            return response.IsSuccessStatusCode ? response.Content.ToString() : " ";
        }

        //=====================================================================================================================
        //                                  P   U   B   L   I   C   A   T   I   O   N   S
        //=====================================================================================================================

        public async Task<List<Publication>> GetPostsAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    MessageBox.Show("Будь ласка, увійдіть в систему.");
                    return new List<Publication>();
                }

                using HttpClient client = new()
                {
                    BaseAddress = new Uri(_baseApiUrl)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await client.GetAsync("api/user/posts");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    try
                    {
                        List<Publication> responseData = JsonConvert.DeserializeObject<List<Publication>>(responseContent);

                        if (responseData != null)
                        {
                            return responseData;
                        }
                        else
                        {
                            MessageBox.Show("Помилка: пуста відповідь від сервера.");
                            return new List<Publication>();
                        }
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show($"Помилка при десеріалізації відповіді сервера: {ex.Message}");
                        return new List<Publication>();
                    }
                }
                else
                {
                    MessageBox.Show($"Помилка при отриманні відповіді від сервера: {response.StatusCode}");
                    return new List<Publication>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
                return new List<Publication>();
            }
        }




        public async Task<string?> PostPostAsync(Publication model)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };
            var response = await client.PostAsJsonAsync("publications", model);
            return response.IsSuccessStatusCode ? response.Content.ToString() : " ";
        }

        public async Task<string?> PutPostAsync(Publication model)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };
            var response = await client.PutAsJsonAsync("publications", model);
            return response.IsSuccessStatusCode ? response.Content.ToString() : " ";
        }

        public async Task<string?> DeletePostAsync(int id)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };
            var response = await client.DeleteAsync($"publications/{id}");
            return response.IsSuccessStatusCode ? response.Content.ToString() : " ";
        }
        //=====================================================================================================================
        //                                               U   S   E   R   S
        //=====================================================================================================================
        public async Task<List<UsersProfile>> GetUsersAsync()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                MessageBox.Show("Будь ласка, увійдіть в систему.");
                return new List<UsersProfile>();
            }

            try
            {
                using HttpClient client = new()
                {
                    BaseAddress = new Uri(_baseApiUrl)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await client.GetAsync("api/User/all_users");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    try
                    {
                        List<UsersProfile> responseData = JsonConvert.DeserializeObject<List<UsersProfile>>(responseContent);

                        if (responseData != null)
                        {
                            return responseData;
                        }
                        else
                        {
                            MessageBox.Show("Помилка: не вдалося десеріалізувати відповідь сервера.");
                            return new List<UsersProfile>();
                        }
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show($"Помилка: {ex.Message}");
                        return new List<UsersProfile>();
                    }
                }
                else
                {
                    MessageBox.Show($"Помилка: {response.StatusCode}");
                    return new List<UsersProfile>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
                return new List<UsersProfile>();
            }
        }


        public async Task<List<UsersProfile>> GetUserAsync()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                MessageBox.Show("Будь ласка, увійдіть в систему.");
                return new List<UsersProfile>();
            }

            try
            {
                using HttpClient client = new()
                {
                    BaseAddress = new Uri(_baseApiUrl)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await client.GetAsync($"api/User/profile/{_userId}"); 

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    try
                    {
                        List<UsersProfile> responseData = JsonConvert.DeserializeObject<List<UsersProfile>>(responseContent);

                        if (responseData != null)
                        {
                            return responseData;
                        }
                        else
                        {
                            MessageBox.Show("Помилка: не вдалося десеріалізувати відповідь сервера.");
                            return new List<UsersProfile>();
                        }
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show($"Помилка: {ex.Message}");
                        return new List<UsersProfile>();
                    }
                }
                else
                {
                    MessageBox.Show($"Помилка: {response.StatusCode}");
                    return new List<UsersProfile>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
                return new List<UsersProfile>();
            }
        }


        //public async Task<string?> PostUserAsync(User model)
        //{
        //    using HttpClient client = new()
        //    {
        //        BaseAddress = new Uri(_baseApiUrl)
        //    };

        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

        //    var response = await client.PostAsJsonAsync("publications", model);
        //    return response.IsSuccessStatusCode ? response.Content.ToString() : " ";
        //}

        public async Task<string?> PutUserAsync(User model)
        {
            using HttpClient client = new()
            {
                BaseAddress = new Uri(_baseApiUrl)
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await client.PutAsJsonAsync("publications", model);
            return response.IsSuccessStatusCode ? response.Content.ToString() : " ";
        }

        //public async Task<string?> DeleteUserAsync(int id)
        //{
        //    using HttpClient client = new()
        //    {
        //        BaseAddress = new Uri(_baseApiUrl)
        //    };

        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

        //    var response = await client.DeleteAsync($"publications/{id}");
        //    return response.IsSuccessStatusCode ? response.Content.ToString() : " ";
        //}
    }
}
