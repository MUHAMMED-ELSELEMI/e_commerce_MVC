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
    public class CategoryRepo : Repo<Category>, ICategoryRepo
    {
        private ApplicationDbContext _db;
        public CategoryRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void update(Category obj)
        {
            _db.Update(obj);

        }
    }
}
