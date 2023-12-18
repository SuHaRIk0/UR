using System.ComponentModel.DataAnnotations;

namespace YouAre.MVVM.Model
{
    public class UsersProfile
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Description { get; set; } = " ";
        public byte[] ProfilePhoto { get; set; } = [];
        public long TimeSpent { get; set; } = 0;

        public static UsersProfile Empty = new() { Id = -1 };
    }
}
