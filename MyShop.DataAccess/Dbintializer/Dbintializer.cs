using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Etuilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Dbintializer
{
    public class Dbintializer : IDbintializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public Dbintializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();        
                }

            }
            catch (Exception)
            {

                throw;
            }

            if (!_roleManager.RoleExistsAsync(DS.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(DS.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(DS.EditorRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(DS.CustomerRole)).GetAwaiter().GetResult();


                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "Admin@Myshop.com",
                    Name = "Administrator",
                    Email = "Admin@Myshop.com",
                    PhoneNumber = "01026498028",
                    Address = "Kafrelshekh",
                    City = "Elhamoul",
                    EmailConfirmed = true
                }, "P@$$w0rd").GetAwaiter().GetResult();

                ApplicationUser user = _context.applicationUser.FirstOrDefault(x=>x.Email == "Admin@Myshop.com");
                _userManager.AddToRoleAsync(user, DS.AdminRole).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
