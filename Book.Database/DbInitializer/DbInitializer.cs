using Book.Models;
using Book.Utilities;
using BookSite.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Database.DbInitializer
{
    public class DbInitializer : IDBInitializer
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext dbContext;
        public DbInitializer(UserManager<IdentityUser> _userManager, 
            RoleManager<IdentityRole> _roleManager, 
            ApplicationDbContext _dbContext)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            dbContext = _dbContext;
        }

        public void Initializer()
        {
            // Push migrations if they are not applied
            try
            {
                if(dbContext.Database.GetPendingMigrations().Count() > 0) {
                    dbContext.Database.Migrate();
                }
            }
            catch(Exception ex)
            {
            }

            // Create roles if they are not created
            if (!roleManager.RoleExistsAsync(StaticDetail.Role_Customer).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(StaticDetail.Role_Customer)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(StaticDetail.Role_Employee)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(StaticDetail.Role_Admin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(StaticDetail.Role_Company)).GetAwaiter().GetResult();
                
                // If roles are not created, then create an admin user as well
                userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@booksite.com",
                    Email = "admin@booksite.com",
                    Name = "Adnim User",
                    PhoneNumber = "1234567890",
                    StreetAddress = "123 Admin Address",
                    State = "NC",
                    PostalCode = "28253",
                    City = "Charlotte"
                },
                "Jingking@12").GetAwaiter().GetResult();

                ApplicationUser user = dbContext.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@booksite.com");
                userManager.AddToRoleAsync(user, StaticDetail.Role_Admin).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
