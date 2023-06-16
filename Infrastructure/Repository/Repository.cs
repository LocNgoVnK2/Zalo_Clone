using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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
        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.Entites.Remove(entity);
                this._context.SaveChanges();

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

        public T GetById(object id)
        {
            return this.Entites.Find(id);
        }

        public void Add(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.Entites.Add(entity);
                this._context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.Entites.Update(entity);
                this._context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

   
    }

}

