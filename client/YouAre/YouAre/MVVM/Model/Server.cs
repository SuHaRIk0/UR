using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
            else
            {

                MessageBox.Show("Один з Параметрів був введений невірно. Сробуйте ще.");

                return null;
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


        public async Task<List<Message>> GetChatAsync(string Username)
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
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/user/chats?user1Id={_userId}&Username={Username}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    try
                    {
                        // Ensure the Chat class has a parameterless constructor
                        List <Message> responseData = JsonConvert.DeserializeObject<List<Message>>(responseContent);

                        if (responseData != null)
                        {
                            //MessageBox.Show($"Server response: {responseData}");
                            return new List<Message>();
                        }
                        else
                        {
                            MessageBox.Show($"Server response: null");
                            return new List<Message>();
                        }
                    }
                    catch (JsonException ex)
                    {
                        // Log the response content and the exception message for debugging purposes
                        Console.WriteLine($"Error deserializing JSON response: {ex.Message}");
                        Console.WriteLine($"Response content: {responseContent}");

                        // Rethrow the exception with a more informative message
                        throw new JsonException("Error deserializing JSON response. See inner exception for details.", ex);
                    }
                }
                else
                {
                    MessageBox.Show($"Server error: {response.StatusCode} - {response.ReasonPhrase}");
                    throw new HttpRequestException($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the general exception
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
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

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonModel = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonModel, Encoding.UTF8, "application/json");

                var response = await client.PostAsJsonAsync($"api/user/send_message", model1);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    // Log or display the successful result if needed
                    //MessageBox.Show($"Server response: {result}");
                    return result;
                }
                else
                {
                    // Log or display the error message if needed
                    MessageBox.Show($"Server error: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception if needed
                MessageBox.Show($"Exception: {ex.Message}");
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
                    //MessageBox.Show($"Server response: {responseContent}");

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


                var model1 = new Publication
                {
                    Text = model.Text,
                    PostAt = DateTime.Now,
                    Picture = model.Picture,
                    AuthorId = _userId,
                };


                //MessageBox.Show($"Before sending: {model1}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonModel = Newtonsoft.Json.JsonConvert.SerializeObject(model1);
                var content = new StringContent(jsonModel, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/user/create_post", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    //MessageBox.Show($"After sending: {model1}");

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
                    MessageBox.Show($"Аватар профілю змінний успішно: {responseContent}");
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
                    MessageBox.Show($"Опис змінено успішно: {responseContent}");
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
