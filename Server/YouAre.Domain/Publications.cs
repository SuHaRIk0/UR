﻿using System.ComponentModel.DataAnnotations;

namespace YouAre.Domain
{
    public class Publication
    {
        public int Id { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public string Picture { get; set; }
        public string Text { get; set; }
        public DateTime PostAt { get; set; }

        public static Publication Empty = new() { Id = -1 };
    }
}