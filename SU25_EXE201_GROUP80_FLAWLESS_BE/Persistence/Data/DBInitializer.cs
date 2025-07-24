using Domain.Constrans;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class DBInitializer
    {
        public async static Task Initialize(UserManager<UserApp> _userManager, RoleManager<IdentityRole> _roleManager, FlawlessDBContext _context)
        {
            var roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Name = UserRole.Admin,
                    NormalizedName = UserRole.Admin.ToUpper(),
                },

                new IdentityRole()
                {
                    Name = UserRole.Artist,
                    NormalizedName = UserRole.Artist.ToUpper(),
                },
                new IdentityRole()
                {
                    Name = UserRole.Customer,
                    NormalizedName = UserRole.Customer.ToUpper(),
                },
            };
            if (!_context.Roles.Any(x => x.Name == UserRole.Admin) 
                && !_context.Roles.Any(x => x.Name == UserRole.Artist) 
                && !_context.Roles.Any(x => x.Name == UserRole.Customer))
            {

                await _context.Roles.AddRangeAsync(roles);
                await _context.SaveChangesAsync();
            }
            if (!_context.UserApps.Any(x => x.Email == "admin@email.com"))
            {
                var user = new UserApp()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "flawlessmobile2025@gmail.com",
                    Name = "admin",
                    TagName = "admin",
                    UserName = "flawlessmobile2025@gmail.com",
                    NormalizedEmail = "FLAWLESSMOBILE2025@GMAIL.COM",
                    NormalizedUserName = "FLAWLESSMOBILE2025@GMAIL.COM",
                    Address = "Location",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };

                await _userManager.CreateAsync(user, "P@ssw0rd");
                await _userManager.SetLockoutEnabledAsync(user, false);

                await _context.SaveChangesAsync();

                var userRole = new IdentityUserRole<string>()
                {
                    UserId = user.Id,
                    RoleId = roles.FirstOrDefault(x => x.Name == UserRole.Admin).Id
                };

                await _userManager.AddToRoleAsync(user, UserRole.Admin);

                await _context.SaveChangesAsync();
            }
        }
    }
}
