using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpensesApplication.Data;
using ExpensesApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ExpensesApplication.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        UserRoles _userRoles;
        public List<SelectListItem> userRole;
        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _userRoles = new UserRoles();
            userRole = new List<SelectListItem>();
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
   
            return View();
        }

       public async Task<List<object[]>> filterAjax(string userName)
        {
            int count = 0, cant;
            string dataFilter = "";
            List<object[]> user = new List<object[]>();
            var appUser = await _context.ApplicationUser.ToListAsync();
            IEnumerable<ApplicationUser> query;
            if (userName == "null")
            {
                query = from u in appUser select u;

            }
            else
            {

                query = from u in appUser where u.UserName.StartsWith(userName) select u;

            }
            cant = query.Count();
            string[] idUser = new string[cant];
            foreach (var Data in query)
            {
                userRole = await _userRoles.getRole(_userManager, _roleManager, Data.Id);
                idUser[count] = Data.Id;
                dataFilter += "<tr>" +
                    "<td>" + Data.Email + "</td>" +
                    "<td>" + Data.EmployeeName + "</td>" +
                    "<td>" + userRole[0].Text + "</td>" +
                    "<td>" + Data.PhoneNumber + "</td>" +
                    "<td>" + Data.UserName + "</td>" +
                    "<td>" +
                    "<a data-toggle='modal' data-target='#myModal' onclick='getDataAjax(" + count + ")' class='btn btn-success'>Edit</a>|" +
                     "<a data-toggle='modal' data-target='#myModal2' onclick='getDataAjax(" + count + ")' class='btn btn-info'>Details</a>|" +
                      "<a data-toggle='modal' data-target='#myModal3' onclick='getDataAjax(" + count + ")' class='btn btn-danger'>Delete</a>|" +
                      "</td>" +
                      "</tr>";
                count++; 
            }
            object[] userData ={dataFilter,idUser};
            user.Add(userData);
            return user;

            }

        



        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccessFailedCount,ConcurrencyStamp,Email,EmailConfirmed,EmployeeName,EmployeeRole,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }


        public async Task<List<Users>> EditAjax(string id)
        {
            List<Users> user = new List<Users>();
            var appUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            userRole = await _userRoles.getRole(_userManager, _roleManager, id);

            user.Add(new Users()
            {

                Id = appUser.Id,
                Email = appUser.Email,
                PhoneNumber = appUser.PhoneNumber,
                UserName = appUser.UserName,
                AccessFailedCount = appUser.AccessFailedCount,
                ConcurrencyStamp = appUser.ConcurrencyStamp,
                EmailConfirmed = appUser.EmailConfirmed,
                LockoutEnabled = appUser.LockoutEnabled,
                LockoutEnd = appUser.LockoutEnd,
                NormalizedEmail = appUser.NormalizedEmail,
                NormalizedUserName = appUser.NormalizedUserName,
                PasswordHash = appUser.PasswordHash,
                PhoneNumberConfirmed = appUser.PhoneNumberConfirmed,
                SecurityStamp = appUser.SecurityStamp,
                TwoFactorEnabled = appUser.TwoFactorEnabled,
                EmployeeName = appUser.EmployeeName,
                RoleId = userRole[0].Value,
                EmployeeRole = userRole[0].Text
                
            });

             
           /* List<ApplicationUser> user = new List<ApplicationUser>();
            var appUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            user.Add(appUser); */

            return user;

        }

        public async Task<String> EditUserAjax(string id, string email, string employeeName, string employeeRole,
            string phoneNumber, string userName, int accessFailedCount, string concurrencyStamp, bool emailConfirmed, bool lockoutEnabled,
            DateTimeOffset lockoutEnd, string normalizedEmail, string normalizedUserName, string passwordHash, bool phoneNumberConfirmed,
            string securityStamp, bool twoFactorEnabled, string selectRole, ApplicationUser applicationUser)
        {
            applicationUser = new ApplicationUser
            {
                Id = id,
                Email = email,
                PhoneNumber = phoneNumber,
                UserName = userName,
                AccessFailedCount = accessFailedCount,
                ConcurrencyStamp = concurrencyStamp,
                EmailConfirmed = emailConfirmed,
                LockoutEnabled = lockoutEnabled,
                LockoutEnd = lockoutEnd,
                NormalizedEmail = normalizedEmail,
                NormalizedUserName = normalizedUserName,
                PasswordHash = passwordHash,
                PhoneNumberConfirmed = phoneNumberConfirmed,
                SecurityStamp = securityStamp,
                TwoFactorEnabled = twoFactorEnabled,
                EmployeeName = employeeName,
                EmployeeRole = employeeRole
            };
            _context.Update(applicationUser);
            await _context.SaveChangesAsync();

            if (selectRole != "No role")
            {
                var user = await _userManager.FindByIdAsync(id);
                userRole = await _userRoles.getRole(_userManager, _roleManager, id);
                if (userRole[0].Text != "No role")
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole[0].Text);
                }
                if (selectRole == "No role")
                {
                    selectRole = "Employee";
                }
                var results = await _userManager.AddToRoleAsync(user, selectRole);
            }
            return "Save";
    }

        public async Task<List<SelectListItem>> RolesAjax()
        {

            List<SelectListItem> rolesList = new List<SelectListItem>();
            rolesList = _userRoles.getRoles(_roleManager);
            return rolesList;

        }

       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,AccessFailedCount,Conc urrencyStamp,Email,EmailConfirmed,EmployeeName,EmployeeRole,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }


        public async Task<String> DeleteUser(string id)
        {

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicationUser.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return "delete";
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}
