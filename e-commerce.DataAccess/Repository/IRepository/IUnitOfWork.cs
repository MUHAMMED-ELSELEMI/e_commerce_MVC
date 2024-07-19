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
        ICategoryRepository Category { get; }

        void save();
    }
}
