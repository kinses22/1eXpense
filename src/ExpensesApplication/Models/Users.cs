using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApplication.Models
{
    public class Users
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeRole { get; set; }
        public string RoleId { get; set; }

        public virtual int AccessFailedCount { get; set; }
        public virtual string ConcurrencyStamp { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual DateTimeOffset? LockoutEnd { get; set; }
        public virtual string NormalizedEmail { get; set; }
        public virtual string NormalizedUserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }

    }
}
