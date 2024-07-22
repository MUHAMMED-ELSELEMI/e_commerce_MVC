using e_commerce.DataAccess.Repository.IRepository;
using ecommerce.DataAccess.Data;
using ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.DataAccess.Repository
{
    public class ShoppingCartRepo : Repo<ShoppingCart>, IShoppingCartRepo
    {
        private ApplicationDbContext _db;
        public ShoppingCartRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void update(ShoppingCart obj)
        {
            _db.shoppingCarts.Update(obj);

        }
    }
}
