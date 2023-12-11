namespace YouAre.Domain
{
    public class Community
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<int>? Members { get; set; }
        public byte[]? CommunityProfilePhoto { get; set; }
        public Community()
        {
            this.Members = new List<int>() { this.AuthorId };
        }

        public static Community Empty = new Community() { Id = -1 };
    }
}
