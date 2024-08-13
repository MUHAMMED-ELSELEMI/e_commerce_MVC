using e_commerce.DataAccess.Repository.IRepository;
using ecommerce.Models;
using ecommerce.Models.ViewModel;
using ecommerce.utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace e_commerce.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Categories
        public IActionResult Index()
        {
            return View(_unitOfWork.Product.GetAll(includeProperties: "Category").ToList());
        }

        // GET: Categories/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _unitOfWork.Product
                .GetFirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Categories/Create
        public IActionResult Upsert(int? id)
        {

            ProductVM vm = new()
            {

                CategoryList = _unitOfWork.Category
             .GetAll().Select(u => new SelectListItem
             {
                 Text = u.Name,
                 Value = u.Id.ToString()
             }),

                product = new Product()
            };

            if (id == null || id == 0)
            {
                //create
                return View(vm);
            }
            else
            {   //update
                vm.product = _unitOfWork.Product.GetFirstOrDefault(m => m.Id == id);
                return View(vm);
            }
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\products");

                    if (!string.IsNullOrEmpty(productVM.product.ImageUrl))
                    {
                        var OldImagePath = Path.Combine(wwwRootPath, productVM.product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(OldImagePath))
                        {
                            System.IO.File.Delete(OldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.product.ImageUrl = @"\images\products\" + fileName;
                }
                if (productVM.product.Id == 0)
                {
                    _unitOfWork.Product.add(productVM.product);
                }
                else
                {
                    _unitOfWork.Product.update(productVM.product);
                }
                _unitOfWork.save();
                TempData["success"] = "record is created successfully";
                return RedirectToAction("Index");

            }

            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }


        #region API CALLS 

        [HttpGet]
        public IActionResult GetAll()
        {

            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return Json(new { data = objProductList });


        }



        [HttpDelete]

        public IActionResult Delete(int? id)
        {
            var ProductToBeDeleted = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (ProductToBeDeleted == null)
            {
                return Json(new { success = false, Message = "Error while deleting" });

            }

            var OldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                                            ProductToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(OldImagePath))
            {
                System.IO.File.Delete(OldImagePath);
            }

            _unitOfWork.Product.Delete(ProductToBeDeleted);
            _unitOfWork.save();

            return Json(new { success = true, Message = "deleted successfully" });


        }
        #endregion

    }
}

