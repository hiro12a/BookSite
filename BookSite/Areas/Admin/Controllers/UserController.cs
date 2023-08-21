using Book.Database.Repository.IRepository;
using Book.Models;
using Book.Models.ViewModels;
using Book.Utilities;
using BookSite.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private UserManager<IdentityUser> _userManager { get; set; }
        public UserController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost, ApplicationDbContext db, UserManager<IdentityUser> userManger) 
        { 
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHost;
            _db = db;
            _userManager = userManger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RoleManagement(string userId)
        {
            // Get roleID
            string roleId = _db.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;

            // Populate RoleManagementVM
            RoleManagementVM roleVM = new RoleManagementVM()
            {
                ApplicationUser = _db.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == userId),
                RoleList = _db.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _db.Companys.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.CompanyId.ToString()
                }),

            };

            // Populate user role
            roleVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == roleId).Name;

            return View(roleVM);
        }
        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
        {
            // Get role Id
            string roleId = _db.UserRoles.FirstOrDefault(u => u.UserId == roleManagementVM.ApplicationUser.Id).RoleId;
            
            // Retrieve old role
            string oldRole = _db.Roles.FirstOrDefault(u => u.Id == roleId).Name;

            // Check if the role is changed
            if(roleManagementVM.ApplicationUser.Role != oldRole)
            {
                // A role has been updated
                ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == roleManagementVM.ApplicationUser.Id);
                
                // Check if the user is from a company, if so then assign one of the companies to the user
                if(roleManagementVM.ApplicationUser.Role == StaticDetail.Role_Company)
                {
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                }

                // If the old role is a company role, remove the company Id since its not a company user anymore
                if(oldRole == StaticDetail.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }

                // Save Changes
                _db.SaveChanges();

                // Remove Old role
                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();

                // Add new role
                _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();
            
                
            }
            return RedirectToAction("Index");
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
