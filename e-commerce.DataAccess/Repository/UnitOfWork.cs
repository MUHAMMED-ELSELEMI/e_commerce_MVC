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
        public ICategoryRepo Category { get; private set; }
        public ICompaniesRepo Companies { get; private set; }
        public IApplicationUserRepo applicationUser { get; private set; }
        public IShoppingCartRepo shoppingCart { get; private set; }
        public IOrderHeaderRepo OrderHeader { get; private set; }
        public IOrderDetailRepo OrderDetail { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepo(_db);
            Product = new ProductRepo(_db);
            Companies = new CompaniesRepo(_db);
            applicationUser = new ApplicationUserRepo(_db);
            shoppingCart = new ShoppingCartRepo(_db);
            OrderHeader = new OrderHeaderRepo(_db);
            OrderDetail = new OrderDetailRepo(_db);
        }


        public void save()
        {
            _db.SaveChanges();
        }
    }
}
