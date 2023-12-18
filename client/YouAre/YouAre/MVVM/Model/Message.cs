using System.ComponentModel.DataAnnotations;

namespace YouAre.MVVM.Model
{
    public class Message
    {
        public int AuthorId { get; set; }
        [Required]
        public int RecipientId { get; set; }
        public string Text { get; set; }
        public DateTime SentAt { get; set; }
    }
}