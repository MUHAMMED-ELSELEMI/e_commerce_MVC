using ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.DataAccess.Repository.IRepository
{
    public interface ICategoryRepo : IRepository<Category>
    {
        void update(Category obj);
    }
}
