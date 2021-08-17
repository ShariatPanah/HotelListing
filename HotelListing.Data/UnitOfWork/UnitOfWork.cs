using HotelListing.Core;
using HotelListing.Data.Repositories;
using System.Threading.Tasks;

namespace HotelListing.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContex)
        {
            _dbContext = dbContex;

            Hotels = new Repository<Hotel>(_dbContext);
            Countries = new Repository<Country>(_dbContext);
        }

        public IRepository<Hotel> Hotels { get; private set; }
        public IRepository<Country> Countries { get; private set; }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
