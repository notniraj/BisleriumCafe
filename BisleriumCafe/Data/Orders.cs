using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    // Represents a cafe order
    public class Orders
    {
        public Guid OrderId { get; set; }
        public List<OrderItems> OrdersList { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerContact { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string StaffUsername { get; set; }
        public double Discount { get; set; } = 0;
        public double Total { get; set; }
        
    }
}

