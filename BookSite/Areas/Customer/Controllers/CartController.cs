using Book.Database.Repository.IRepository;
using Book.Models;
using Book.Models.ViewModels;
using Book.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookSite.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartVM _cartVm;
        public CartController(IUnitOfWork unitOfWork)
        { 
            _unitOfWork = unitOfWork;
        }
        public IActionResult ViewCart()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value; // Populate userId

            _cartVm = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
            };

            foreach(var cart in _cartVm.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                _cartVm.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(_cartVm);
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value; // Populate userId

            _cartVm = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
            };

            _cartVm.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == userId);

            _cartVm.OrderHeader.FirstName = _cartVm.OrderHeader.ApplicationUser.FirstName;
            _cartVm.OrderHeader.LastName = _cartVm.OrderHeader.ApplicationUser.LastName;
            _cartVm.OrderHeader.PhoneNumber = _cartVm.OrderHeader.ApplicationUser.PhoneNumber;
            _cartVm.OrderHeader.City = _cartVm.OrderHeader.ApplicationUser.City;
            _cartVm.OrderHeader.State = _cartVm.OrderHeader.ApplicationUser.State;
            _cartVm.OrderHeader.PostalCode = _cartVm.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in _cartVm.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                _cartVm.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(_cartVm);
        }
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost(CartVM cartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value; // Populate userId

            cartVM.ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product");

            cartVM.OrderHeader.OrderDate = System.DateTime.Now;
            cartVM.OrderHeader.ApplicationUserId = userId;

            ApplicationUser appUser = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == userId);

            foreach (var cart in cartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                cartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);        
            }

            // User GetValueOrDefault because companyId can be null
            if(appUser.CompanyId.GetValueOrDefault() == 0)
            {
                // Regular account and need to capture payment
                cartVM.OrderHeader.PaymentStatus = StaticDetail.PaymentStatusPending;
                cartVM.OrderHeader.OrderStatus = StaticDetail.StatusPending;
            }
            else
            {
                // Company user
                cartVM.OrderHeader.PaymentStatus = StaticDetail.PaymentStatusDelayedPayment;
                cartVM.OrderHeader.OrderStatus = StaticDetail.StatusApproved;
            }
            _unitOfWork.OrderHeaderRepository.Add(cartVM.OrderHeader);
            _unitOfWork.Save();

            foreach(var cart in cartVM.ShoppingCartList)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = cartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetailsRepository.Add(orderDetails);
                _unitOfWork.Save();
            }

            // Capture Payment
            // User GetValueOrDefault because companyId can be null
            if (appUser.CompanyId.GetValueOrDefault() == 0)
            {
                // Regular account and need to capture payment
                // Stripe logic
            }

            // Pass the Id if needed here. Can do this here instead of the view 
            return RedirectToAction(nameof(OrderConfirmation), new { id = cartVM.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }


        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.Get(u => u.ProductId == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCartRepository.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(ViewCart));
        }
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.Get(u => u.ProductId == cartId);
            if(cartFromDb.Count <= 1) 
            {
                // Remove from cart
                _unitOfWork.ShoppingCartRepository.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCartRepository.Update(cartFromDb);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(ViewCart));
        }
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.Get(u => u.ProductId == cartId);
            _unitOfWork.ShoppingCartRepository.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(ViewCart));
        }

        //Calculate Price
        private double GetPriceBasedOnQuantity(ShoppingCart cart)
        {
            if(cart.Count <= 50)
            {
                return cart.Product.Price;
            }
            else if(cart.Count <= 100)
            {
                return cart.Product.Price50;
            }
            else if(cart.Count > 100)
            {
                return cart.Product.Price100;
            }
            else
            {
                return 0;
            }
        }
    }
}
