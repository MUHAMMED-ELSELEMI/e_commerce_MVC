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
	public class OrderHeaderRepo : Repo<OrderHeader>, IOrderHeaderRepo
	{
		private ApplicationDbContext _db;
		public OrderHeaderRepo(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}


		public void update(OrderHeader obj)
		{
			_db.Update(obj);

		}

		public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
		{
			var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
			if (orderFromDb != null)
			{
				orderFromDb.OrderStatus = orderStatus;
				if (!string.IsNullOrEmpty(paymentStatus) )
				{
					orderFromDb.PaymentStatus = paymentStatus;
				}
			}
		}

		public void UpdateStripePaymentID(int id, string SessionId, string paymentIntentId)
		{
			var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
			if (!string.IsNullOrEmpty(SessionId))
			{
				orderFromDb.SessionId = SessionId;
				orderFromDb.paymentIntentId = paymentIntentId;
			}
			if (!string.IsNullOrEmpty(paymentIntentId))
			{
				orderFromDb.paymentIntentId = paymentIntentId;
				orderFromDb.PaymentDate = DateTime.Now;
			}
		}
	}
}
