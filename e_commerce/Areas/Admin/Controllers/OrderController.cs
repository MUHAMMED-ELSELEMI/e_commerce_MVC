using e_commerce.DataAccess.Repository.IRepository;
using ecommerce.Models;
using ecommerce.Models.ViewModel;
using ecommerce.utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace e_commerce.Areas.Admin.Controllers
{

    [Area("Admin")]


    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetail.GetAll(o => o.OrderHeaderId == orderId, includeProperties: "Product")
            };

            return View(orderVM);
        }

        #region API CALLS 

        [HttpGet]
        public IActionResult GetAll(string status )
        {
            IEnumerable<OrderHeader> objOrderList = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();

            switch (status)
            {
                case "pending":
                 objOrderList = objOrderList.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":

                    objOrderList = objOrderList.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":

                    objOrderList = objOrderList.Where(u => u.PaymentStatus == SD.StatusShipped);
                    break;
                case "approved":

                    objOrderList = objOrderList.Where(u => u.PaymentStatus == SD.StatusApproved);
                    break;
                default: 
                    break;
            }

            return Json(new { data = objOrderList });


        }
    }

        #endregion


 
}
