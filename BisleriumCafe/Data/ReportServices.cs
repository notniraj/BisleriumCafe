using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    // Service class for generating reports based on order data
    public class ReportServices
    {
        // Order services instance for data retrieval
        private OrderServices _orderServices { get; set; }

        // Constructor for initializing dependencies
        public ReportServices(OrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        // Generate a list of orders for a specific report type and date
        public List<Orders> GenerateOrderList(string reportType, string reportDate)
        {
            // Retrieve all orders from the order services
            List<Orders> orders = _orderServices.GetAllOrders();

            // Filter orders based on the specified report type and date
            if (reportType.ToLower() == "monthly")
            {
                orders = orders.Where(order => reportDate == order.OrderDate.ToString("MM-yyyy")).ToList();
            }
            else if (reportType.ToLower() == "daily")
            {
                orders = orders.Where(order => reportDate == order.OrderDate.ToString("dd-MM-yyyy")).ToList();
            }

            // Return the filtered list of orders
            return orders;
        }
    }
}
