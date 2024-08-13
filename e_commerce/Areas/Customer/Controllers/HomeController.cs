using e_commerce.DataAccess.Repository.IRepository;
using ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace e_commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger , IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category"); 
            return View(productList);
        }


        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category");
            if (product == null)
            {
                return NotFound();
            }

            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = product,
                Count = 1,
                ProductId = id.Value
            };

            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCart.ApplicationUserId = userId;

            // Ensure the Id is not set
            shoppingCart.Id = 0;

            ShoppingCart cartFromDb = _unitOfWork.shoppingCart.GetFirstOrDefault(
                u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

            if (cartFromDb != null)
            {
                // If the product already exists in the cart, update the quantity
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.shoppingCart.update(cartFromDb);
            }
            else
            {
                // If the product does not exist in the cart, add a new entry
                _unitOfWork.shoppingCart.add(shoppingCart);
            }

            _unitOfWork.save();
            TempData["success"] = "Item added to cart successfully";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}