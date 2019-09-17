using BookStore.Hateoas;
using FluentValidation;
using System;

namespace BookStore.Models
{
    public class Quote
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Decsription { get; set; }

        public string Author { get; set; }

        public string Type { get; set; }

        public DateTime CreateAt { get; set; }
    }

    public class QuoteValidator : AbstractValidator<Quote>
    {
        public QuoteValidator()
        {
            RuleFor(x => x.Title).NotNull();
            RuleFor(x => x.Decsription).NotNull().Length(1, 100);
            RuleFor(x => x.Author).NotNull().Length(1, 30);
            RuleFor(x => x.Type).NotNull();
            RuleFor(x => x.CreateAt).NotNull();
        }
    }
}