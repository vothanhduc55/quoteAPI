using BookStore.Interfaces;
using System.Threading.Tasks;

namespace BookStore.Data
{
    public class UnitOfWork : IUnitofWork
    {
        private readonly QuotesDbContext _quotesDbContext;

        public UnitOfWork(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }

        public async Task<bool> SaveAsync()
        {
            return await _quotesDbContext.SaveChangesAsync() > 0;
        }
    }
}