using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static MudBlazor.Icons;

namespace BisleriumCafe.Data
{
    // Service class for managing customer memberships and related operations
    public class MembershipService
    {
        private OrderServices _orderServices;

        // Constructor for MembershipService, requires an instance of OrderServices
        public MembershipService(OrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        // Retrieves all customers from the stored data
        public static List<Customers> GetAllCustomer()
        {
            string CustomerListFilePath = Utils.GetCustomersListFilePath();
            if (!File.Exists(CustomerListFilePath))
            {
                return new List<Customers>();
            }
            var json = File.ReadAllText(CustomerListFilePath);

            return JsonSerializer.Deserialize<List<Customers>>(json);
        }

        // Saves the provided list of customers to the data storage
        public void SaveAllCustomer(List<Customers> customerList)
        {
            string appDirectoryPath = Utils.GetAppDirectoryPath();
            string CustomerListFilePath = Utils.GetCustomersListFilePath();

            if (!Directory.Exists(appDirectoryPath))
            {
                Directory.CreateDirectory(appDirectoryPath);
            }

            var json = JsonSerializer.Serialize(customerList);
            File.WriteAllText(CustomerListFilePath, json);
        }

        // Retrieves a customer by their contact information
        public Customers GetCustomerByContact(string customerContact)
        {
            List<Customers> customerList = GetAllCustomer();
            Customers customer = customerList.FirstOrDefault(x => x.CustomerContact == customerContact);
            return customer;
        }

        // Creates a new membership for the provided customer
        public void CreateMembership(Customers customer)
        {
            Customers ExistingCustomer = GetCustomerByContact(customer.CustomerEmail);
            List<Customers> customers = GetAllCustomer();

            if (ExistingCustomer != null)
            {
                throw new Exception("Customer has membership");
            }

            customers.Add(customer);
            SaveAllCustomer(customers);

        }

        // Changes the order count for a customer
        public void ChangeOrderCount(string customerEmail)
        {
            List<Customers> customers = GetAllCustomer();
            Customers customer = customers.FirstOrDefault(x => x.CustomerEmail == customerEmail);

            SaveAllCustomer(customers);
        }

        // Calculates the number of free coffees a customer is eligible for based on their order count
        public int FreeCoffeeCount(string customerPhone)
        {
            List<Orders> orders = _orderServices.GetAllOrders();
            int totalCount = orders.Where(order => order.CustomerContact == customerPhone).ToList().Count();

            return totalCount / 10;
        }

        // Checks if a customer is a regular customer based on monthly order count
        public bool IsRegularCustomer(string customerContact)
        {
            List<Orders> orders = _orderServices.GetAllOrders();

            int month = DateTime.Now.Month - 1;
            int year = month == 12 ? DateTime.Now.Year - 1 : DateTime.Now.Year;

            int totalNoOfOrders = orders.Where(order => order.CustomerContact == customerContact && order.OrderDate.Month == month && order.OrderDate.Year == year)
                .GroupBy(order => order.OrderDate.Day)
                .ToList().Count();
            return totalNoOfOrders > 26;
        }


        // Updates the redeemed free coffee count for a customer
        public void UpdateRedeemedFreeCoffeeCount(string customerContact, int redeemedFreeCoffeeCount)
        {
            List<Customers> customers = GetAllCustomer();
            Customers customer = customers.FirstOrDefault(c => c.CustomerContact == customerContact);
            customer.RedeemCount = redeemedFreeCoffeeCount;

            SaveAllCustomer(customers);
        }
    }
}
