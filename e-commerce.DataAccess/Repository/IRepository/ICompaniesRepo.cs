using ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.DataAccess.Repository.IRepository
{
    public interface ICompaniesRepo : IRepository<Companies>
    {
        public void update(Companies obj);
        
    }
}
