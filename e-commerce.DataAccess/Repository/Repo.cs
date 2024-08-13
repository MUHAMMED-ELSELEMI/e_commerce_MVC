using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using e_commerce.DataAccess.Repository.IRepository;
using ecommerce.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.DataAccess.Repository
{
    public class Repo<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbset;
        public Repo(ApplicationDbContext db)
        {
            _db = db;
            dbset = _db.Set<T>();
            _db.products.Include(u => u.Category);
        }
        public void add(T entity)
        {
            dbset.Add(entity);
        }

        public void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {

                foreach (var property in includeProperties.
                    Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);

                }
            }
            return query.ToList();

        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;

            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {

                foreach (var property in includeProperties.
                    Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);

                }
            }
            return query.FirstOrDefault();
        }




    }

}
