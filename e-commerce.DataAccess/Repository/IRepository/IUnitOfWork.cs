using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductRepo Product { get; }
        ICategoryRepo Category { get; }
        ICompaniesRepo Companies { get; }
        IApplicationUserRepo applicationUser { get; }
        IShoppingCartRepo shoppingCart { get; }
        IOrderHeaderRepo OrderHeader { get; }
        IOrderDetailRepo OrderDetail { get; }

        void save();
    }
}
