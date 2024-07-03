using SE160271.BookManagementApplicationRepo.Models;
using SE160271.BookManagementApplicationRepo.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE160271.BookManagementApplicationRepo.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Book> Books { get; }
        IGenericRepository<Press> Presses { get; }
        IGenericRepository<Address> Addresses { get; }
        Task SaveAsync();

        Task<int> CompleteAsync();
    }

}
