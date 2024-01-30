using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    // Represents an individual item within a cafe order
    public class OrderItems
    {
        // identifier for the order item
        public Guid OrderId { get; set; } = Guid.NewGuid();
        // identifier for the cafe item associated with this order item
        public Guid ItemId { get; set; }
        // Name of the cafe item
        public string ItemName { get; set; }
        // Type or category of the cafe item
        public string ItemType { get; set; }
        // Quantity of the cafe item in the order
        public int ItemAmount { get; set; }
        // Price per unit of the cafe item
        public double ItemPrice { get; set; }
        // Total price for the quantity of cafe items in this order item
        public double OrderTotalPrice { get; set; }
    }
}
