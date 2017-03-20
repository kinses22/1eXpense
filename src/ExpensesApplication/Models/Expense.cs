using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpensesApplication.Models
 {
  
    public class Expense
    {
        public int ID { get; set; }

        [Display(Name= "Username")]
        public string UserName { get; set; }

        [Display(Name = "Project Code"),RegularExpression(@"^([a-zA-Z0-9_\s]+)*$", ErrorMessage = "Use alphanumeric characters only"), Required, StringLength(10)]
        public string ProjectCode { get; set; }

        [Display(Name = "Expense Type")]
        public string Expensetype { get; set; } // (General/Miles/Perdiem/creditCard) //drop down menu

        [Display(Name= "CC Expense")]
        public string CCExpenseType { get; set; }//(flight, hotel, NA) //drop down menu, hidden unless ExpenseType = credit card

        [Required, Range(0, 999.99)]
        public decimal Amount { get; set; }

        public string Currency { get; set; } //drop down (euro, pound)

        public string Image { get; set; } //url of image, need an upload link beside this

        [Display(Name = "Expense Status")]
        public string ExpenseStatus { get; set; } //approved, pending, rejected

        [Display(Name = "Rejection Info")]
        public string RejectionInfo { get; set; } //hidden unless the expenseStatus is set to rejected, can be left blank

        [Display(Name = "Expense Date"),DataType(DataType.Date),DateVal(), Required]
        public DateTime ExpenseDate { get; set; } //the date the expense took place

        [Display(Name = "Submitted")]
        public DateTime SubmitDate { get; set; } //the date the user submitted the expense

        public bool Chargeable { get; set; }//y/n? //tick box
        /*
        -	ID 
        -	Project Code
        -	Expensetype: (General/Miles/Perdiem/creditCard) //drop down menu
        -   creditCardExpenseType(flight,hotel) //drop down menu, hidden unless ExpenseType = credit card
        -	Amount
        -	Currency
        -	Images
        -	Expense Status
        -	Date of expense
        -	Date it is submitted
        -	Chargeable y/n? //tick box
        */

    }
}
