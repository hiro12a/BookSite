using Book.Models;
using BookSite.Database;
using Microsoft.AspNetCore.Mvc;

namespace BookSite.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Create Category 
        public IActionResult CategoryView()
        {
            IEnumerable<Category> objCategories = _db.Categories;
            return View(objCategories);
        }
        public IActionResult CreateCategory(Category cat)
        {
            if(cat.Name == cat.DisplayOder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name");
            }
            if(ModelState.IsValid)
            {
                _db.Categories.Add(cat);
                _db.SaveChanges();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("CategoryView");
            }
            return View();
        }

        // Edit Category 
        public IActionResult EditCategory(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _db.Categories.FirstOrDefault(u=>u.Id==id);
            if(categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult EditCategory(Category cat)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(cat);
                _db.SaveChanges();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("CategoryView");
            }
            return View();
        }

        // Edit Category 
        public IActionResult DeleteCategory(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _db.Categories.FirstOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("DeleteCategory")]
        public IActionResult SaveDeleteCategory(int? id)
        {
            Category? cat = _db.Categories.FirstOrDefault(c => c.Id==id);
            if(cat == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(cat);
            _db.SaveChanges();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("CategoryView");
        }
    }
}
