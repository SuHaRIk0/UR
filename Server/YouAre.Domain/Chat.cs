namespace YouAre.Domain
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<int>? Members { get; set; }

        public static Chat Empty = new() { Id = -1 };
    }
}
