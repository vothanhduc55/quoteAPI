﻿using BookStore.Hateoas;

namespace BookStore.DTO
{
    public class QuoteDTO : LinkResourceBase
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Decsription { get; set; }

        public string Author { get; set; }

        public string Type { get; set; }
    }
}