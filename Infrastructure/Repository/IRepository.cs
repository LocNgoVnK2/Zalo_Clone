using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.Repository
{
    public interface IRepository<T>
    {
        Task<T> GetById(object id);
        IQueryable<T> GetAll();
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<IDbContextTransaction> BeginTransaction();
    }
}
