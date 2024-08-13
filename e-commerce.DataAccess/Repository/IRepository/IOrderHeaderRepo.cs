using ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepo : IRepository<OrderHeader>
    {
        void update(OrderHeader obj);
        void UpdateStatus(int id , string orderStatus , string? paymentStatus = null );
        void UpdateStripePaymentID (int id , string SessionId, string paymentIntentId );

    }
}
