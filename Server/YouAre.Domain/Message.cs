namespace YouAre.Domain
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public string? Text { get; set; }
        public byte[]? Picture { get; set; }
        public static Message Empty = new() { Id = -1 };
    }
}
