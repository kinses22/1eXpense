using ExpensesApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApplication.ViewModels
{
    public class ProjectExpenseViewModel
    {
        public List<Expense> Expenses { get; set; }
        public List<Project> Projects { get; set; }
        public List<Customer> Customers { get; set; }
        public int projectID { get; set; }
        public string customerTable { get; set; }
    }
}
