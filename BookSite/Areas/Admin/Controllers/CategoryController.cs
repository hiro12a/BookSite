﻿using Book.Database;
using Microsoft.AspNetCore.Mvc;
using BookSite.Database;
using Book.Models;
using Book.Database.Repository.IRepository;

namespace BookSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Create Category 
        public IActionResult CategoryView()
        {
            IEnumerable<Category> objCategories = _unitOfWork.CategoryRepository.GetAll().ToList();
            return View(objCategories);
        }
        public IActionResult CreateCategory(Category cat)
        {
            if (cat.Name == cat.DisplayOder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Add(cat);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("CategoryView");
            }
            return View();
        }

        // Edit Category 
        public IActionResult EditCategory(int? catId)
        {
            if (catId == null || catId == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.CatId == catId);
            if (categoryFromDb == null)
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
                _unitOfWork.CategoryRepository.Update(cat);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("CategoryView");
            }
            return View();
        }

        // Edit Category 
        public IActionResult DeleteCategory(int? catId)
        {
            if (catId == null || catId == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.CatId == catId);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("DeleteCategory")]
        public IActionResult SaveDeleteCategory(int? catId)
        {
            Category? cat = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.CatId == catId);
            if (cat == null)
            {
                return NotFound();
            }
            _unitOfWork.CategoryRepository.Remove(cat);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("CategoryView");
        }
    }
}