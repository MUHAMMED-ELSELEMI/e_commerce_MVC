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
    public class CompaniesRepo : Repo<Companies>, ICompaniesRepo 
    {
        private ApplicationDbContext _db;
        public CompaniesRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


      public  void update(Companies obj)
        {
            _db.Companies.Update(obj);

        }
    }
}
