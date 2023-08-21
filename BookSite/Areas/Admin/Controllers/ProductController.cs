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
                productVM.Product = _unitOfWork.ProductRepository.Get(u => u.ProductId == id, includeProperties: "ImageManagers"); // Image manager must be the same name as in applicationdbcontext
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM prod, List<IFormFile> files)
        {
            if(ModelState.IsValid)
            {
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

                string wwwRootPath = _webHostEnvironment.WebRootPath; // Get root path
                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {
                        string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // Gives random name for file
                        string productpath = @"Image\Products\Product-" + prod.Product.ProductId;
                        string finalPath = Path.Combine(wwwRootPath, productpath); // Get path

                        // Create folder if it does not exist
                        if (!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }

                        // Upload image
                        using (var fileStream = new FileStream(Path.Combine(finalPath, filename), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        // Save Image
                        ImageManager imageManager = new()
                        {
                            ImageUrl = @"\" + productpath + @"\" + filename,
                            ProductId = prod.Product.ProductId,
                        };

                        // Populate image in a list
                        if(prod.Product.ImageManagers == null)
                        {
                            prod.Product.ImageManagers = new List<ImageManager>();
                        }

                        prod.Product.ImageManagers.Add(imageManager);
                        _unitOfWork.ImageManagerRepository.Add(imageManager);                     
                    }

                    _unitOfWork.ProductRepository.Update(prod.Product);
                    _unitOfWork.Save();
                }
                TempData["success"] = "Product created or updated successfully";
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

            /*var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, prodToDelete.ImageManagers.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }*/

            _unitOfWork.ProductRepository.Remove(prodToDelete);
            _unitOfWork.Save();

            List<Product> products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = products });
        }
        #endregion

    }
}
