using System.ComponentModel.DataAnnotations;

namespace YouAre.MVVM.Model
{
    public class Message
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        [Required]
        public int RecipientId { get; set; }
        public string Text { get; set; }
        public bool IsReaded { get; set; }

        public DateTime SentAt { get; set; }
    }

    public class DisplayMassage
    {
        [Required]
        public string Author { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime SentAt { get; set; }
    }
}