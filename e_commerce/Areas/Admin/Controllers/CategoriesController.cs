using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ecommerce.Models;
using ecommerce.DataAccess.Data;
using e_commerce.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using ecommerce.utility;
namespace e_commerce.Areas.Admin.Controllers

{
    [Area("Admin")]
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Categories
        public IActionResult Index()
        {
            return View(_unitOfWork.Category.GetAll().ToList());
        }

        // GET: Categories/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _unitOfWork.Category
                .GetFirstOrDefault(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,DisplayOrder")] Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.add(category);
                _unitOfWork.save();
                TempData["success"] = "record is created successfully";

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _unitOfWork.Category.GetFirstOrDefault(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,DisplayOrder")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Category.update(category);
                    _unitOfWork.save();
                    TempData["success"] = "record is updated successfully";


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _unitOfWork.Category
                .GetFirstOrDefault(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }


            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _unitOfWork.Category.GetFirstOrDefault(m => m.Id == id);
            if (category != null)
            {
                _unitOfWork.Category.Delete(category);
            }

            _unitOfWork.save();
            TempData["success"] = "record is deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            var obje = _unitOfWork.Category.GetFirstOrDefault(e => e.Id == id);
            if (!(obje == null))
            {
                return true;
            }


            return false;
        }
    }
}
