using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApplication.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string Manager { get; set; }
        public string ProjectCode { get; set; }
        public string Practice { get; set; } //drop down menu: bas, msp etc.
        public string Customer { get; set; }
    }
}
