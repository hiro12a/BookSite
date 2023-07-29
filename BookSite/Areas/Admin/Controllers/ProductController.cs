using Book.Database.Repository.IRepository;
using Book.Models;
using Book.Models.ViewModels;
using BookSite.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment; // For Image Upload
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost) 
        { 
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHost;
        }

        public IActionResult ViewProduct()
        {
            List<Product> products = _unitOfWork.ProductRepository.GetAll(includeProperties:"Category").ToList(); // Get list of product                
            return View(products);
        }

        // Create Product
        public IActionResult Upsert(int? id) // Combination of Update and Insert
        {
            // Using ViewModels instead of ViewBags/ViewData
            ProductVM productVM = new()
            {
                //  Get Category while using SelectListItem
                CategoryList = _unitOfWork.CategoryRepository.
                GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CatId.ToString(),
                }),
                Product = new Product()

            };

            if (id == null || id == 0)
            {
                // Create
                return View(productVM);
            }
            else
            {
                // Update, only return one record
                productVM.Product = _unitOfWork.ProductRepository.Get(u => u.ProductId == id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM prod, IFormFile? file)
        {
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath; // Get root path
                if(file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // Gives random name for file
                    string productPath = Path.Combine(wwwRootPath, @"Images\Products\"); // Get path
                    
                    if (!string.IsNullOrEmpty(prod.Product.ImageUrl)) // Checks if image is not null
                    {
                        // Delete Old image
                        var oldImagePath = 
                            Path.Combine(wwwRootPath, prod.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save File upload
                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    prod.Product.ImageUrl = @"\Images\Products\" + filename;
                }
                // Check whether to add or update              
                if (prod.Product.ProductId == 0)
                {
                    _unitOfWork.ProductRepository.Add(prod.Product);
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(prod.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("ViewProduct"); // Return to Product Homepage
            }
            else
            {
                prod.CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value  = u.CatId.ToString(),
                });
            }
            return View();
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll(int? id)
        {
            List<Product> products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = products});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var prodToDelete = _unitOfWork.ProductRepository.Get(u => u.ProductId == id);
            if(prodToDelete == null)
            {
                return Json(new { success = false, message = "Erorr while Deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, prodToDelete.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.ProductRepository.Remove(prodToDelete);
            _unitOfWork.Save();

            List<Product> products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = products });
        }
        #endregion

    }
}
