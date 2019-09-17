using BookStore.Data;
using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Repositories
{
    public class QuoteRepostitory : IQuoteRepository
    {
        public readonly QuotesDbContext _quotesDbContext;

        public QuoteRepostitory(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }

        public void addQuote(Quote quote)
        {
            quote.CreateAt = DateTime.Now;
            _quotesDbContext.Quotes.Add(quote);
            _quotesDbContext.SaveChanges();
        }

        public void deleteQuote(Quote quote)
        {
            //var quote = _quotesDbContext.Quotes.Find(id);
            _quotesDbContext.Quotes.Remove(quote);
        }

        public Quote GetQuoteById(int id)
        {
            return _quotesDbContext.Quotes.Find(id);
        }

        public IEnumerable<Quote> GetQuotes()
        {
            return _quotesDbContext.Quotes.ToList();
        }

        public void updateQuote(Quote quote)
        {
             //var quote = _quotesDbContext.Quotes.Find(id);
            _quotesDbContext.Quotes.Update(quote);
        }
    }
}