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
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.Net.Http.Headers;
using System.Collections;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ExpensesApplication.ViewModels;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;

namespace ExpensesApplication.Controllers
{

    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;

        public DateTime SubmitDate { get; private set; }
        public string ExpenseStatus { get; private set; }
        public string RejectionInfo { get; private set; }


        public ExpensesController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Write to CSV File for all expenses
        public FileContentResult ExportToCSV()
        {
            var myExport = new CsvExport();

            foreach (var field in _context.Expense) {
                myExport.AddRow();
                myExport["User"] = field.UserName;
                myExport["Submitted"] = field.SubmitDate;
                myExport["Rejection Info"] = field.RejectionInfo;
                myExport["Project Code"] = field.ProjectCode;
                myExport["Type"] = field.Expensetype;
                myExport["Status"] = field.ExpenseStatus;
                myExport["Expense Date"] = field.ExpenseDate;
                myExport["Chargeable"] = field.Chargeable;
                myExport["Expense Type"] = field.CCExpenseType;
                myExport["Amount"] = field.Amount;

            }
            return File(myExport.ExportToBytes(), "text/csv", "results.csv");

        }

        // Write to CSV File for one person expenses
        public FileContentResult ExportToCSVForOneUser()
        {
            var myExport = new CsvExport();

            foreach (var field in _context.Expense)
            {
                myExport.AddRow();
                myExport["User"] = field.UserName;
                myExport["Submitted"] = field.SubmitDate;
                myExport["Rejection Info"] = field.RejectionInfo;
                myExport["Project Code"] = field.ProjectCode;
                myExport["Type"] = field.Expensetype;
                myExport["Status"] = field.ExpenseStatus;
                myExport["Expense Date"] = field.ExpenseDate;
                myExport["Chargeable"] = field.Chargeable;
                myExport["Expense Type"] = field.CCExpenseType;
                myExport["Amount"] = field.Amount;

            }
            return File(myExport.ExportToBytes(), "text/csv", "results.csv");

        }







        // GET: Expenses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Expense.ToListAsync());
        }

        public async Task<IActionResult> Index2()
        {
            return View(await _context.Expense.ToListAsync());
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense.SingleOrDefaultAsync(m => m.ID == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }


        // GET: Expenses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Amount,CCExpenseType,Chargeable,Currency,ExpenseDate,Expensetype,Image,ProjectCode")] Expense expense, ICollection<IFormFile> files)
        {
            //okay so the form on the create page is not taking into account the 
            //I need to go through this entire code block to understand what is going wrong
            expense.SubmitDate = DateTime.Now;
            expense.ExpenseStatus = "pending";
            expense.RejectionInfo = "";
            expense.UserName = HttpContext.User.Identity.Name;

            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            string allfiles = "";

            foreach (var file in files)
            {
                allfiles = allfiles + file.FileName;

                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            expense.Image = allfiles;
            //why does the screen go blank when I hit upload? what is the difference between this project and the last one?
            if (ModelState.IsValid) //up to here I think I'm okay
            {

                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index2");
            }
            return View(expense);
        }


        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense.SingleOrDefaultAsync(m => m.ID == id);
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
            //return RedirectToAction("CustomersProjects","Projects");
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("ID,Amount,CCExpenseType,Chargeable,Currency,ExpenseDate,ExpenseStatus,Expensetype,Image,ProjectCode,RejectionInfo,SubmitDate,UserName")] Expense expense)
        public async Task<IActionResult> Edit(int id, [Bind("ID,Chargeable,ExpenseStatus,SubmitDate,RejectionInfo")] Expense model)
        { 
        //    if (id != expense.ID)
        //    {
        //        return NotFound();
        //    }

            var expense = await _context.Expense.SingleOrDefaultAsync(m => m.ID == id);
            if (expense == null)
            {
                return NotFound();
            }
            expense.SubmitDate = DateTime.Now;
            expense.Chargeable = model.Chargeable;
            expense.ExpenseStatus = model.ExpenseStatus;
            if (model.ExpenseStatus=="approved")
            {
                expense.RejectionInfo = "";
            }
            else
            {
                expense.RejectionInfo = model.RejectionInfo;
            }

            try
            {
                _context.Update(expense);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(expense.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            //before we return we need to send an email confirming the approval
            #region smtp old
            /*

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("myusername@gmail.com", "mypwd"),
                EnableSsl = true
            };
            client.Send("myusername@gmail.com", "myusername@gmail.com", "test", "testbody");
            //
            */
            #endregion

            #region smtp new

            /*
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Joey Tribbiani", "joey@friends.com"));
            message.To.Add(new MailboxAddress("Mrs. Chanandler Bong", "chandler@friends.com"));
            message.Subject = "How you doin'?";

            message.Body = new TextPart("plain")
            {
                Text = @"Hey Chandler,I just wanted to let you know that Monica and I were going to go play some paintball, you in?-- Joey"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.friends.com", 587, false);

                // Note: since we don't have an OAuth2 token, disable the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("joey", "password");

                client.Send(message);
                client.Disconnect(true);
            }
            */
            #endregion
            //end of the email stuff here

            //return RedirectToAction("Index2");
            return RedirectToAction("CustomersProjects", "Projects");
            //return View(expense);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expense.SingleOrDefaultAsync(m => m.ID == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expense = await _context.Expense.SingleOrDefaultAsync(m => m.ID == id);
            _context.Expense.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index2");
        }

        public ActionResult Download()
        {
            string[] files = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "uploads"));
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }

            return View(files);
            //return View(movie);
        }

        public ActionResult ProjectExpences()
        {
            List<Expense> expences = _context.Expense.ToList();
            List<Project> projects = _context.Project.ToList();

            var viewModel = new ProjectExpenseViewModel()
            {
                Expenses = expences,
                Projects = projects

            };

            return View(viewModel);
        }

        public FileResult DownloadFile(string fileName)
        {
            var filepath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, "application/x-msdownload", fileName);
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expense.Any(e => e.ID == id);
        }
    }
}
