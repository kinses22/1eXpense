using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpensesApplication.Data;
using ExpensesApplication.Models;
using Microsoft.AspNetCore.Authorization;
using ExpensesApplication.ViewModels;

namespace ExpensesApplication.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Project.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.SingleOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Customer,Manager,Practice,ProjectCode")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.SingleOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Customer,Manager,Practice,ProjectCode")] Project project)
        {
            if (id != project.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ID))
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
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.SingleOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.SingleOrDefaultAsync(m => m.ID == id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult CustomersProjects(string customertable)
        {
            List<Expense> expences = _context.Expense.ToList();
            List<Project> projects = _context.Project.ToList();
            List<Customer> customers = _context.Customer.ToList();

            var viewModel = new ProjectExpenseViewModel()
            {
                Expenses = expences,
                Projects = projects,
                Customers = customers,
                customerTable = customertable

            };
            var selectListItems = new List<SelectListItem>();
            selectListItems = viewModel.Customers.Select(s => new SelectListItem { Text = s.Name, Value = s.Name }).ToList();
            ViewBag.SelectItem = selectListItems;
            return View(viewModel);
            
        }

        public ActionResult CustomersProjects()
        {
            List<Expense> expences = _context.Expense.ToList();
            List<Project> projects = _context.Project.ToList();
            List<Customer> customers = _context.Customer.ToList();

            var viewModel = new ProjectExpenseViewModel()
            {
                Expenses = expences,
                Projects = projects,
                Customers = customers
                //customerTable = "Microsoft"

            };
            var selectListItems = new List<SelectListItem>();
            selectListItems= viewModel.Customers.Select(s => new SelectListItem { Text = s.Name, Value = s.Name }).ToList();
            ViewBag.SelectItem = selectListItems;
            return View(viewModel);
        }

        public async Task<IActionResult> ProjectExpenseDetails(int? id)
        {
            //viewmodel part
            List<Expense> expences = _context.Expense.ToList();
            List<Project> projects = _context.Project.ToList();
            List<Customer> customers = _context.Customer.ToList();

            var viewModel = new ProjectExpenseViewModel()
            {
                Expenses = expences,
                Projects = projects,
                Customers = customers,
                projectID = (int)id

            };
            //viewmodel part end

            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.SingleOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            //return View(project);

            return View(viewModel);
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ID == id);
        }

        public FileContentResult ExportToCSV()
        {
            var myExport = new CsvExport();

            foreach (var field in _context.Project)
            {
                myExport.AddRow();
                myExport["Project Code"] = field.ProjectCode;
                myExport["Customer"] = field.Customer;
                myExport["Manager"] = field.Manager;
                myExport["Practice"] = field.Practice;


            }
            return File(myExport.ExportToBytes(), "text/csv", "projects.csv");

        }


    }
}
