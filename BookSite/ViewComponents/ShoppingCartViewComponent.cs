using Book.Database.Repository.IRepository;
using Book.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookSite.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); // Populate userId
            if (userId != null)
            {
                if (HttpContext.Session.GetInt32(StaticDetail.SessionCart) == null)
                {
                    HttpContext.Session.SetInt32(StaticDetail.SessionCart,
                    _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId.Value).Count());
                }
                
                return View(HttpContext.Session.GetInt32(StaticDetail.SessionCart));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }

    }
}
