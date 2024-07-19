using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using e_commerce.DataAccess.Repository.IRepository;
using ecommerce.DataAccess.Data;
using ecommerce.Models;

namespace e_commerce.DataAccess.Repository
{
    public class ProductRepo : Repo<Product>, IProductRepo
    {
        private ApplicationDbContext _context;
        public ProductRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public void update(Product obj)
        {
            _context.Update(obj);

        }

    }
}
