using Book.Database.Repository.IRepository;
using Book.Models;
using Book.Models.ViewModels;
using Book.Utilities;
using BookSite.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetail.Role_Admin)] // Make sure only admins can access it
    public class UserController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private ApplicationDbContext _db;
        private IWebHostEnvironment _webHostEnvironment; // For Image Upload
        public UserController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost) 
        { 
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHost;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            // Mapping tables
            List<ApplicationUser> users = _db.ApplicationUsers.Include(u=>u.Company).ToList();

            var userRoles = _db.UserRoles.ToList();

            foreach(var user in users)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;

                if(user.Company == null)
                {
                    user.Company = new()
                    {
                        Name = ""
                    };
                }
            }
            return Json(new {data = users});
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

            List<ApplicationUser> users = _unitOfWork.ApplicationUserRepository.GetAll().ToList();
            return Json(new { data = users });
        }
        #endregion

    }
}
