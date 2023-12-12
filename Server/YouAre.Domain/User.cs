using System;
using System.ComponentModel.DataAnnotations;

namespace YouAre.Domain
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Login { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public long TimeSpent { get; set; } = 0;

        public static User Empty = new() { Id = -1 };
    }
}