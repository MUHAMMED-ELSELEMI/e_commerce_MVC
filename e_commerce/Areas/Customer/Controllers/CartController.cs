using e_commerce.DataAccess.Repository.IRepository;
using ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ecommerce.Models.ViewModel;
using System.Security.Claims;
using ecommerce.utility;
using e_commerce.DataAccess.Repository;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
namespace e_commerce.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		[BindProperty]
		public ShoppingCartVM shoppingCartVM { get; set; }
		public CartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			shoppingCartVM = new ShoppingCartVM()
			{
				ListCart = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
				OrderHeader = new OrderHeader()

			};

			foreach (var cart in shoppingCartVM.ListCart)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

			return View(shoppingCartVM);
		}


		public ActionResult Summary()
		{


			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			shoppingCartVM = new ShoppingCartVM()
			{
				ListCart = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
				OrderHeader = new OrderHeader()

			};

			shoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.applicationUser.GetFirstOrDefault(u => u.Id == userId);

			shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.ApplicationUser.Name;
			shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
			shoppingCartVM.OrderHeader.StreetAddress = shoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
			shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City;
			shoppingCartVM.OrderHeader.State = shoppingCartVM.OrderHeader.ApplicationUser.State;
			shoppingCartVM.OrderHeader.PostalCode = shoppingCartVM.OrderHeader.ApplicationUser.PostalCode;


			foreach (var cart in shoppingCartVM.ListCart)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}
			return View(shoppingCartVM);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Summary(ShoppingCartVM shoppingCartVM)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			shoppingCartVM.ListCart = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");

			shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
			shoppingCartVM.OrderHeader.ApplicationUserId = userId;

			ApplicationUser applicationUser = _unitOfWork.applicationUser.GetFirstOrDefault(u => u.Id == userId);

			foreach (var cart in shoppingCartVM.ListCart)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

			if (applicationUser.CompanyID.GetValueOrDefault() == 0)
			{
				shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
				shoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
			}
			else
			{
				shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
				shoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
			}


			_unitOfWork.OrderHeader.add(shoppingCartVM.OrderHeader);
			_unitOfWork.save();

			foreach (var cart in shoppingCartVM.ListCart)
			{
				OrderDetail orderDetail = new OrderDetail()
				{
					ProductId = cart.ProductId,
					OrderHeaderId = shoppingCartVM.OrderHeader.Id,
					Price = cart.Price,
					Count = cart.Count
				};
				_unitOfWork.OrderDetail.add(orderDetail);
				_unitOfWork.save();
			}
			if (applicationUser.CompanyID.GetValueOrDefault() == 0)
			{
				//stripe settings 
				var domain = "https://localhost:7195/";
				var options = new Stripe.Checkout.SessionCreateOptions
				{
					SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={shoppingCartVM.OrderHeader.Id}",
					CancelUrl = domain + "Customer/Cart/Index",
					LineItems = new List<SessionLineItemOptions>(),
					Mode = "payment",
				};


				foreach (var item in shoppingCartVM.ListCart)
				{
					var sessionltem = new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = (long)item.Price * 100,
							Currency = "usd",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.Product.Title
							},
						},
						Quantity = item.Count
					};
					options.LineItems.Add(sessionltem);
				}

				var service = new SessionService();
				Session session = service.Create(options);
				_unitOfWork.OrderHeader.UpdateStripePaymentID(
				shoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
				_unitOfWork.save();
				Response.Headers.Add("Location", session.Url);
				return new StatusCodeResult(303);


			}



			return RedirectToAction("OrderConfirmation", "Cart", new { id = shoppingCartVM.OrderHeader.Id });
		}

		public IActionResult OrderConfirmation(int id)
		{
			OrderHeader OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser");
			if (OrderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
			{
				var service = new SessionService();
				Session session = service.Get(OrderHeader.SessionId);

				if (session.PaymentStatus.ToLower() == "paid")
				{

					_unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);

					_unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
					_unitOfWork.save();

				}

			}

			List<ShoppingCart> shoppingCarts = _unitOfWork.shoppingCart.
				GetAll(c => c.ApplicationUserId == OrderHeader.ApplicationUserId).ToList();

			_unitOfWork.shoppingCart.DeleteRange(shoppingCarts);
			_unitOfWork.save();

			return View(id);

		}

		public IActionResult Plus(int cartId)
		{
			var cart = _unitOfWork.shoppingCart.GetFirstOrDefault(c => c.Id == cartId, includeProperties: "Product");
			cart.Count += 1;
			cart.Price = GetPriceBasedOnQuantity(cart);
			_unitOfWork.save();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Minus(int cartId)
		{
			var cart = _unitOfWork.shoppingCart.GetFirstOrDefault(c => c.Id == cartId);
			if (cart.Count == 1)
			{
				_unitOfWork.shoppingCart.Delete(cart);
			}
			else
			{
				cart.Count -= 1;
				_unitOfWork.shoppingCart.update(cart);
			}

			_unitOfWork.save();
			return RedirectToAction(nameof(Index));
		}


		public IActionResult Remove(int cartId)
		{
			var cart = _unitOfWork.shoppingCart.GetFirstOrDefault(c => c.Id == cartId);

			_unitOfWork.shoppingCart.Delete(cart);

			_unitOfWork.save();

			return RedirectToAction(nameof(Index));
		}


		private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
		{
			if (shoppingCart.Count <= 50)
			{
				return shoppingCart.Product.Price;
			}
			else if (shoppingCart.Count <= 100)
			{
				return shoppingCart.Product.Price50;
			}
			else
			{
				return shoppingCart.Product.Price100;
			}

		}
	}
}
