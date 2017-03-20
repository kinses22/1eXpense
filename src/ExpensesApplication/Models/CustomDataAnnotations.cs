using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExpensesApplication.Models
{
    //public class DateRangeValidate : ValidationAttribute
    //{
    //    public DateRangeValidate()
    //    {

    //    }

    //    public override ValidationResult IsValid(object value)
    //    {
    //        var dt = (DateTime)value;
    //        if (DateTime.Now.Subtract(dt).TotalDays > 1)
    //        {
    //            var errorMessage = FormatErrorMessage("Date cannot be greater than 60 days old");
    //            return new ValidationResult(errorMessage);
    //        }

    //        return ValidationResult.Success;
    //    }
    //}

    public class DateVal : ValidationAttribute
    {
        private readonly int _days;

        public DateVal()
        {
           // _days = days;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                    var dateval = (DateTime)value;
                    int days = 60;
                    if (DateTime.Now.Subtract(dateval).TotalDays > days)
                    {
                        var errorMessage ="Date cannot be more than "+days+" ago.";
                        return new ValidationResult(errorMessage);
                    }
                if (DateTime.Now < dateval)
                {
                    var errorMessage = "Date cannot be after today";
                    return new ValidationResult(errorMessage);

                }
                
            }
            return ValidationResult.Success;
        }
    }

}
