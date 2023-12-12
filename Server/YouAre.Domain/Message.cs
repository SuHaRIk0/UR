using System.ComponentModel.DataAnnotations;

namespace YouAre.Domain
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int RecipientId { get; set; }
        public string Text { get; set; }
        public DateTime SentAt { get; set; }

        public static Message Empty = new() { Id = -1 };
    }
}