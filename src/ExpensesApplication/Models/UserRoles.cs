using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApplication.Models
{
    public class UserRoles
    {

        public List<SelectListItem> userRoles;
        public UserRoles()
        {
            userRoles = new List<SelectListItem>();
        }

        public List<SelectListItem> getRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = roleManager.Roles.ToList();
            foreach (var Data in roles)
            {
                userRoles.Add(new SelectListItem()
                {
                    Value = Data.Id,
                    Text = Data.Name

                });
            }
            return userRoles; 
        }

        public async Task<List<SelectListItem>> getRole(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, string ID)
        {

            userRoles = new List<SelectListItem>();
            string role;
            var users = await userManager.FindByIdAsync(ID);
            var roles = await userManager.GetRolesAsync(users);
            if (roles.Count == 0)
            {
                userRoles.Add(new SelectListItem()
                {
                    Value = "Null",
                    Text = "No Role"
                });
            }
            else
            {
                role = Convert.ToString(roles[0]);
                var rolesID = roleManager.Roles.Where(m => m.Name == role);
                foreach (var Data in rolesID)
                {
                    userRoles.Add(new SelectListItem()
                    {
                        Value = Data.Id,
                        Text = Data.Name
                    });

                }

            }
            return userRoles;
        }
    }
}
