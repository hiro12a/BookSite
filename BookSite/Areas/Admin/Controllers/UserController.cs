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
        public UserController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost, ApplicationDbContext db) 
        { 
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHost;
            _db = db;
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
            var roles = _db.Roles.ToList();

            foreach(var user in users)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u=>u.Id == roleId).Name;


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

        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id) // From body is from the database table
        {
            // Directly work with entity framework core
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if(objFromDb == null)
            {
                return Json(new { success = false, message = "Error While Locking/Unlocking" });
            }
            
            if(objFromDb.LockoutEnd != null && objFromDb.LockoutEnd> DateTime.Now)
            {
                // User is currently locked and needs to be unlocked
                objFromDb.LockoutEnd = DateTime.Now; // User will be unlocked
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            _db.SaveChanges();
            

            return Json(new { success = true, message = "Operation Sucecssful"});
        }
        #endregion

    }
}
