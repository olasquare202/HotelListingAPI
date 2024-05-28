using HotelListingAPI.Data;
using HotelListingAPI.IRepository;
using HotelListingAPI.Model;

namespace HotelListingAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _databaseContext;
        private IGenericRepository<Country> _countries;
        private IGenericRepository<Hotel> _hotels;

        public UnitOfWork(DatabaseContext databaseContext) 
        {
            _databaseContext = databaseContext;
        }
        public IGenericRepository<Country> Countries => _countries ??= new GenericRepository<Country>(_databaseContext);

        public IGenericRepository<Hotel> Hotels => _hotels ??= new GenericRepository<Hotel>(_databaseContext);

        public void Dispose()//it does garbage collector function(i.e after d operation, it free up the memory).
        {
            _databaseContext.Dispose();//dispose d memory after d operation
            GC.SuppressFinalize(this);//GC means Garbage Collector
        }

        public async Task Save()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
