using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    // Class representing a collection of reports for cafe transactions
    public class Reports
    {
        public string ReportType { get; set; }
        public string ReportDate { get; set; }
        public List<Orders> OrdersList { get; set; }
        public List<OrderItems> CoffeeList { get; set; }
        public List<OrderItems> AddInsList { get; set; }
        public double Revenue { get; set; } = 0;
        
    }
}
