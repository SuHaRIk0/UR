using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
        public async Task<List<Chat>> GetChatsAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return new List<Chat>();
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
                        List<Chat> responseData = JsonConvert.DeserializeObject<List<Chat>>(responseContent);

                        if (responseData != null)
                        {
                            return responseData;
                        }
                        else
                        {
                            return new List<Chat>();
                        }
                    }
                    catch (JsonException ex)
                    {
                        return new List<Chat>();
                    }
                }
                else
                {
                    return new List<Chat>();
                }
            }
            catch (Exception ex)
            {
                return new List<Chat>();
            }
        }


        public async Task<Chat> GetChatAsync(int user2Id)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    return null;
                }

                using HttpClient client = new()
                {
                    BaseAddress = new Uri(_baseApiUrl)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await client.GetAsync($"api/user/chats?user1Id={_userId}&user2Id={user2Id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    try
                    {
                        Chat responseData = JsonConvert.DeserializeObject<Chat>(responseContent);

                        if (responseData != null)
                        {
                            return responseData;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (JsonException ex)
                    {
                        return null;
                    }
                }
                else
                {
                    // Якщо є помилка HTTP, можливо, ви хочете генерувати виняток замість виведення MessageBox
                    throw new HttpRequestException($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        //=====================================================================================================================
        //                                          M  E  S  S   A   G   E   S
        //=====================================================================================================================
        public async Task<string?> PostMessageAsync(Message model)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    MessageBox.Show("Будь ласка, увійдіть в систему.");
                    return null;
                }

                using HttpClient client = new()
                {
                    BaseAddress = new Uri(_baseApiUrl)
                };

                var model1 = new Message
                {
                    AuthorId = _userId,
                    RecipientId = model.RecipientId,
                    Text = model.Text,
                };

                var response = await client.PostAsJsonAsync($"api/user/send_message", model1);

                return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
            }
            catch (Exception ex)
            {
                return null;
            }
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
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    MessageBox.Show("Будь ласка, увійдіть в систему.");
                    return null;
                }

                using HttpClient client = new()
                {
                    BaseAddress = new Uri(_baseApiUrl)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonModel = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonModel, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/user/create_post", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    MessageBox.Show($"Помилка при відправці посту: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error Content: {errorContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
                return null;
            }
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


        public async Task<string?> EditAvaAsync(UsersProfile model)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    MessageBox.Show("Будь ласка, увійдіть в систему.");
                    return null;
                }

                using HttpClient client = new()
                {
                    BaseAddress = new Uri(_baseApiUrl)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonModel = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonModel, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("update_avatar", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Avatar updated successfully: {responseContent}");
                }
                else
                {
                    MessageBox.Show($"Error updating avatar: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error Content: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return null; // Add this line to address the compilation error
        }

        public async Task<string?> EditDescAsync(UsersProfile model)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    MessageBox.Show("Будь ласка, увійдіть в систему.");
                    return null;
                }

                using HttpClient client = new()
                {
                    BaseAddress = new Uri(_baseApiUrl)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonModel = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonModel, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("update_description", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"description updated successfully: {responseContent}");
                }
                else
                {
                    MessageBox.Show($"Error updating description: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error Content: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return null; // Add this line to address the compilation error
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
        public async Task<UsersProfile> GetUserAsync()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                MessageBox.Show("Будь ласка, увійдіть в систему.");
                return UsersProfile.Empty;
            }

            try
            {
                using HttpClient client = new()
                {
                    BaseAddress = new Uri(_baseApiUrl)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                var response = await client.GetAsync($"api/User/profile?userId={_userId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    try
                    {
                        UsersProfile userProfile = JsonConvert.DeserializeObject<UsersProfile>(responseContent);

                        if (userProfile != null)
                        {
                            return userProfile;
                        }
                        else
                        {
                            MessageBox.Show("Помилка: не вдалося десеріалізувати відповідь сервера.");
                            return UsersProfile.Empty;
                        }
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show($"Помилка: {ex.Message}");
                        return UsersProfile.Empty;
                    }
                }
                else
                {
                    MessageBox.Show($"Помилка: {response.StatusCode}");
                    return UsersProfile.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
                return UsersProfile.Empty;
            }
        }
    }
}
