using e_commerce.DataAccess.Repository.IRepository;
using ecommerce.Models;
using ecommerce.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace e_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CompaniesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompaniesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Categories
        public IActionResult Index()
        {
            return View(_unitOfWork.Companies.GetAll().ToList());
        }

        // GET: Categories/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _unitOfWork.Companies
                .GetFirstOrDefault(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Categories/Create
        public IActionResult Upsert(int? id)
        {
            Companies companies = new Companies();

            if (id == null || id == 0)
            {
                //create
                return View(companies);
            }
            else
            {   //update
                Companies company = _unitOfWork.Companies.GetFirstOrDefault(m => m.CompanyId == id);
                return View(company);
            }
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]   
        public IActionResult Upsert(Companies company)
        {
            if (ModelState.IsValid)
            {
                if (company.CompanyId == 0)
                {
                    _unitOfWork.Companies.add(company);
                    TempData["success"] = "Company created successfully";
                }
                else
                {
                    _unitOfWork.Companies.update(company);
                    TempData["success"] = "Company updated successfully";
                }
                _unitOfWork.save();
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }





        #region API CALLS 

        [HttpGet]
        public IActionResult GetAll()
        {

            List<Companies> objCompaniesList = _unitOfWork.Companies.GetAll().ToList();

            return Json(new { data = objCompaniesList });


        }



        [HttpDelete]

        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Companies.GetFirstOrDefault(u => u.CompanyId == id);

            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, Message = "Error while deleting" });

            }

            _unitOfWork.Companies.Delete(CompanyToBeDeleted);
            _unitOfWork.save();
            TempData["success"] = "record is deleted successfully";
            return Json(new { success = true, Message = "deleted successfully" });



        }
        #endregion

    }
}

