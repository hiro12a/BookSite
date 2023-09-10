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
        private UserManager<IdentityUser> _userManager { get; set; }
        private RoleManager<IdentityRole> _roleManager { get; set; }
        public UserController(IUnitOfWork unitOfWork,RoleManager<IdentityRole> roleManger, UserManager<IdentityUser> userManger) 
        { 
            _unitOfWork = unitOfWork;
            _userManager = userManger;
            _roleManager = roleManger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RoleManagement(string userId)
        {
            // Populate RoleManagementVM
            RoleManagementVM RoleVM = new RoleManagementVM()
            {
                ApplicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == userId, includeProperties:"Company"),
                RoleList = _roleManager.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _unitOfWork.CompanyRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.CompanyId.ToString()
                }),

            };

            // Automatically retrieve and populate user role 
            RoleVM.ApplicationUser.Role = _userManager.GetRolesAsync(_unitOfWork.ApplicationUserRepository.Get(u=>u.Id == userId)).
                GetAwaiter().GetResult().FirstOrDefault();

            return View(RoleVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
        {
            // Retrieve old role
            string oldRole = _userManager.GetRolesAsync(_unitOfWork.ApplicationUserRepository.Get(u => u.Id == roleManagementVM.ApplicationUser.Id)).
                GetAwaiter().GetResult().FirstOrDefault();

            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == roleManagementVM.ApplicationUser.Id);

            // Check if the role is changed
            if (!(roleManagementVM.ApplicationUser.Role == oldRole))
            {

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
                _unitOfWork.ApplicationUserRepository.Update(applicationUser);
                _unitOfWork.Save();

                // Remove Old role
                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();

                // Add new role
                _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();                         
            }
            // Check if only company is updated 
            else
            {
                if (oldRole == StaticDetail.Role_Company && applicationUser.CompanyId != roleManagementVM.ApplicationUser.CompanyId)
                {
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                    applicationUser.Name = roleManagementVM.ApplicationUser.Name;
                    applicationUser.PhoneNumber = roleManagementVM.ApplicationUser.PhoneNumber;
                    applicationUser.StreetAddress = roleManagementVM.ApplicationUser.StreetAddress;
                    applicationUser.City = roleManagementVM.ApplicationUser.City;
                    applicationUser.State = roleManagementVM.ApplicationUser.State;
                    applicationUser.PostalCode = roleManagementVM.ApplicationUser.PostalCode;
                    _unitOfWork.ApplicationUserRepository.Update(applicationUser);
                    _unitOfWork.Save();
                }
            }

            return RedirectToAction("Index");
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            // Mapping tables
            List<ApplicationUser> users = _unitOfWork.ApplicationUserRepository.GetAll(includeProperties:"Company").ToList();


            foreach(var user in users)
            {
                
                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();

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
            var objFromDb = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == id);
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

            _unitOfWork.ApplicationUserRepository.Update(objFromDb);
            _unitOfWork.Save();
            

            return Json(new { success = true, message = "Operation Sucecssful"});
        }
        #endregion

    }
}
