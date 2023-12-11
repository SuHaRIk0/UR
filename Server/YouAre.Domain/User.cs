global using System.ComponentModel.DataAnnotations;

namespace YouAre.Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Login { get; set; }
        public byte[]? ProfilePhoto { get; set; }

        public static User Empty = new() { Id = -1 };
    }
}
