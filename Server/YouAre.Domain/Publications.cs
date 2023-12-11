namespace YouAre.Domain
{
    public class Publication
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public byte[]? Picture { get; set; }
        public string? Text { get; set; }

        public static Publication Empty = new () { Id = -1 };
    }
}
