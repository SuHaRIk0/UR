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
        public string Description { get; set; } = "Default description";
        public string ProfilePhoto { get; set; } = "https://images.unsplash.com/photo-1602271886918-bafecc837c7a?q=80&w=1000&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8c29tZXxlbnwwfHwwfHx8MA%3D%3D";
        public long TimeSpent { get; set; } = 0;

        public static User Empty = new() { Id = -1 };
    }
}