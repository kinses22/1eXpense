﻿using ExpensesApplication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApplication.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Employee Type")]
        public List<SelectListItem> Roles { get; set; }
        public string EmployeeRole { get; set; }
        private UserRoles userRoles;


        public RegisterViewModel()
        {
            Roles = new List<SelectListItem>();
            userRoles = new UserRoles();
        }

        public void getRoles(RoleManager<IdentityRole> roleManager)
        {
            Roles = userRoles.getRoles(roleManager);

        }


        public string EmployeeName { get; set; }
    }
}
