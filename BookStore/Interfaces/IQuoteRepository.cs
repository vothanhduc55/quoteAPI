using BookStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Interfaces
{
    public interface IQuoteRepository
    {
        IEnumerable<Quote> GetQuotes();

        void updateQuote(Quote quote);

        void deleteQuote(Quote quote);

        void addQuote(Quote quote);

        Quote GetQuoteById(int id);
    }
}