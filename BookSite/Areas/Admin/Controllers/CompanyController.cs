using Book.Database.Repository.IRepository;
using Book.Models;
using Book.Models.ViewModels;
using Book.Utilities;
using BookSite.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetail.Role_Admin)] // Make sure only admins can access it
    public class CompanyController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment; // For Image Upload
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost) 
        { 
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHost;
        }

        public IActionResult ViewCompany()
        {
            List<Company> Companys = _unitOfWork.CompanyRepository.GetAll().ToList(); // Get list of Company                
            return View(Companys);
        }

        // Create Company
        public IActionResult Upsert(int? id) // Combination of Update and Insert
        {
            if (id == null || id == 0)
            {
                // Create
                return View(new Company());
            }
            else
            {
                // Update, only return one record
                Company Company = _unitOfWork.CompanyRepository.Get(u => u.CompanyId == id);
                return View(Company);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company comp)
        {
            if(ModelState.IsValid)
            {
                // Check whether to add or update              
                if (comp.CompanyId == 0)
                {
                    _unitOfWork.CompanyRepository.Add(comp);
                }
                else
                {
                    _unitOfWork.CompanyRepository.Update(comp);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("ViewCompany"); // Return to Company Homepage
            }
            else
            {
                return View(comp);
            }
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll(int? id)
        {
            List<Company> Companys = _unitOfWork.CompanyRepository.GetAll().ToList();
            return Json(new {data = Companys});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var prodToDelete = _unitOfWork.CompanyRepository.Get(u => u.CompanyId == id);
            if(prodToDelete == null)
            {
                return Json(new { success = false, message = "Erorr while Deleting" });
            }

            _unitOfWork.CompanyRepository.Remove(prodToDelete);
            _unitOfWork.Save();

            List<Company> Companys = _unitOfWork.CompanyRepository.GetAll().ToList();
            return Json(new { data = Companys });
        }
        #endregion

    }
}
