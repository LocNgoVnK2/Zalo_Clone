using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ZaloDbContext _context;
        public Repository(ZaloDbContext context)
        {
            this._context = context;
        }
        private DbSet<T> _entities;
        private DbSet<T> Entites
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }
                return _entities;
            }
        }
        public async Task<bool> Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.Entites.Remove(entity);
                await this._context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
                
            }
        }

        public IQueryable<T> GetAll()
        {
            return this.Entites;
        }

        public async Task<T> GetById(object id)
        {
            return await this.Entites.FindAsync(id);
        }

        public async Task<bool> Add(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                await this.Entites.AddAsync(entity);
                await this._context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.Entites.Update(entity);
                await this._context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }

}

