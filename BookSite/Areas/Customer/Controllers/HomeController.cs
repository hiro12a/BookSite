using Book.Database.Repository.IRepository;
using Book.Models;
using Book.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BookSite.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unit)
        {
            _logger = logger;
            _unitOfWork = unit;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category,ImageManagers");
            return View(products);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.ProductRepository.Get(u => u.ProductId == id, includeProperties: "Category,ImageManagers"),
                Count = 1,
                ProdId = id
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize] // Make sure user is logged in
        public IActionResult Details(ShoppingCart cart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value; // Populate userId
            cart.ApplicationUserId = userId;


            // Check for existing item
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCartRepository.Get(u => u.ApplicationUserId == userId && 
            u.ProdId == cart.ProdId);
            if(cartFromDb != null)
            {
                // Shopping cart exist
                cartFromDb.Count += cart.Count; // Ads onto cart
                _unitOfWork.ShoppingCartRepository.Update(cartFromDb);
            }
            else
            {
                // Add Cart
                _unitOfWork.ShoppingCartRepository.Add(cart);

                // Set session to display a number for the cart icon
                HttpContext.Session.SetInt32(StaticDetail.SessionCart,
                    _unitOfWork.ShoppingCartRepository.GetAll(u=>u.ApplicationUserId == userId).Count());
                
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}