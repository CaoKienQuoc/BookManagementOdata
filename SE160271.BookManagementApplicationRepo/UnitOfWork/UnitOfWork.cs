using SE160271.BookManagementApplicationRepo.Models;
using SE160271.BookManagementApplicationRepo.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE160271.BookManagementApplicationRepo.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookManagementDBContext _context;
        private IGenericRepository<Book> _books;
        private IGenericRepository<Press> _presses;
        private IGenericRepository<Address> _addresses;

        public UnitOfWork(BookManagementDBContext context)
        {
            _context = context;
        }

        public IGenericRepository<Book> Books => _books ??= new GenericRepository<Book>(_context);
        public IGenericRepository<Press> Presses => _presses ??= new GenericRepository<Press>(_context);
        public IGenericRepository<Address> Addresses => _addresses ??= new GenericRepository<Address>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }

}
