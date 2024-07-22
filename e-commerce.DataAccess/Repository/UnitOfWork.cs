using e_commerce.DataAccess.Repository.IRepository;
using ecommerce.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IProductRepo Product { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public ICompaniesRepo Companies { get; private set; }
        public IShoppingCartRepo shoppingCart { get; private set; }
        public IApplicationUserRepo applicationUser { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepo(_db);
            Companies = new CompaniesRepo(_db);
            shoppingCart = new ShoppingCartRepo(_db);
            applicationUser = new ApplicationUserRepo(_db);
        }


        public void save()
        {
            _db.SaveChanges();
        }
    }
}
