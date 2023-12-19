using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace YouAre.MVVM.Model
{
    public class UsersProfile
    {
        [JsonProperty("userId")]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Description { get; set; } = " ";
        public string ProfilePhoto { get; set; } = " ";
        public string IdText { get; set; } = " ";
        public int UnreadedMessages { get; set; }
        public long TimeSpent { get; set; } = 0;

        public static UsersProfile Empty = new() { Id = -1 };
    }
}
