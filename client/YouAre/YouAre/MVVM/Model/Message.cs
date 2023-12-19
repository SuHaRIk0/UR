using System.ComponentModel.DataAnnotations;

namespace YouAre.MVVM.Model
{
    public class Message
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "AuthorId is required.")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "RecipientId is required.")]
        public int RecipientId { get; set; }

        [Required(ErrorMessage = "Text is required.")]
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