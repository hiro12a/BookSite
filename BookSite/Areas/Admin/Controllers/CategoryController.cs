using Book.Database;
using Microsoft.AspNetCore.Mvc;
using BookSite.Database;
using Book.Models;
using Book.Database.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Book.Utilities;
using Book.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe;

namespace BookSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetail.Role_Admin)] // Make sure only admins can access it
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult CategoryView()
        {
            IEnumerable<Category> objCategories = _unitOfWork.CategoryRepository.GetAll().ToList();
            return View(objCategories);
        }

        // Create Category 
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                // Create
               return View(new Category());
            
            }
            else
            {
                // Update, only return one record
                Category Category = _unitOfWork.CategoryRepository.Get(u => u.CatId == id);
                return View(Category);
            }

        }
        [HttpPost]
        public IActionResult Upsert(Category cat)
        {
            if (ModelState.IsValid)
            {
                if (cat.Name == cat.DisplayOder.ToString())
                {
                    ModelState.AddModelError("name", "The Display Order cannot exactly match the Name");
                }

                if (cat.CatId == 0)
                {
                    _unitOfWork.CategoryRepository.Add(cat);
                }
                else
                {
                    _unitOfWork.CategoryRepository.Update(cat);
                }

                _unitOfWork.Save();

                return RedirectToAction("CategoryView");

            }           
            return View();
        }

        // Delete Category 
        public IActionResult DeleteCategory(int? catId)
        {
            if (catId == null || catId == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _unitOfWork.CategoryRepository.Get(u => u.CatId == catId);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("DeleteCategory")]
        public IActionResult SaveDeleteCategory(int? catId)
        {
            Category? cat = _unitOfWork.CategoryRepository.Get(c => c.CatId == catId);
            if (cat == null)
            {
                return NotFound();
            }
            _unitOfWork.CategoryRepository.Remove(cat);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("CategoryView");
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll(int? id)
        {
            List<Category> categories = _unitOfWork.CategoryRepository.GetAll().ToList();
            return Json(new { data = categories });
        }
        #endregion

    }
}
