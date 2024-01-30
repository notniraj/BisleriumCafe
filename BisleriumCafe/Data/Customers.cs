using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    // Represents a customer in a cafe
    public class Customers
    {
        // Unique identifier for the customer
        public Guid CustomerID { get; set; } = Guid.NewGuid();
        // Name of the customer
        public string CustomerName { get; set; }
        // Email address of the customer
        public string CustomerEmail { get; set; }
        // Contact number of the customer
        public string CustomerContact { get; set; }
        // Address of the customer
        public string CustomerAddress { get; set; }
        // Count of redeemable items associated with the customer
        public int RedeemCount { get; set; } = 0;
        // Indicates whether the customer has a membership
        public bool HasMembership { get; set; } = false;
        

    }
}
