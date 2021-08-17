using HotelListing.Core;
using HotelListing.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace HotelListing.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<Hotel> Hotels { get; }
        public IRepository<Country> Countries { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
