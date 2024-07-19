using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecommerce.Models;


namespace e_commerce.DataAccess.Repository.IRepository
{
    public interface IProductRepo : IRepository<Product>
    {
        public void update(Product obj);


    }

}
