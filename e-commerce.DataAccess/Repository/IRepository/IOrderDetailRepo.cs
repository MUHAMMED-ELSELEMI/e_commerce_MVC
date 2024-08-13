using ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepo : IRepository<OrderDetail>
    {
        void update(OrderDetail obj);
    }
}
