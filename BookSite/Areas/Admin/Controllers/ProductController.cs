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
                CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem(
                    text: u.Name,
                    value: u.CatId.ToString()
                )),
                Product = new Product()
            };
            if(id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                // Update
                productVM.Product = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.ProductId == id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM prod, IFormFile? file)
        {
            if(ModelState.IsValid)
            {
                string rootPath = _webHostEnvironment.WebRootPath; // Get root path
                if(file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // Gives random name for file
                    string productPath = Path.Combine(rootPath, @"Images\Products\"); // Get path
                    
                    if (!string.IsNullOrEmpty(prod.Product.ImageUrl)) // Checks if image is not null
                    {
                        // Delete Old image
                        var oldImagePath = Path.Combine(rootPath, prod.Product.ImageUrl.TrimStart('\\'));
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
                if(prod.Product.ProductId == 0)
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

            return View();
        }

        // Delete Product
        public IActionResult DeleteProduct(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? product = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.ProductId == id); // Find product by ID
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpPost, ActionName("DeleteProduct")]
        public IActionResult SaveDeleteProduct(int? id)
        {
            Product? product = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.ProductId == id);
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Remove(product);
                _unitOfWork.Save();
                return RedirectToAction("ViewProduct");
            }
            return View();
        }

    }
}
