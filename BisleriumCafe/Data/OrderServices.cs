using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    // Service class for managing cafe orders
    public class OrderServices
    {
        // Add an order item to the list or update the quantity and total price if it already exists
        public void AddToOrderItemsList(List<OrderItems> _orderItems, Guid ItemID, string OrderItemName, double OrderItemPrice)
        {
            OrderItems orderItem = _orderItems.FirstOrDefault(x => x.ItemId.ToString() == ItemID.ToString());

            if (orderItem != null)
            {
                orderItem.ItemAmount++;
                orderItem.OrderTotalPrice = orderItem.ItemAmount * OrderItemPrice;
                return;
            }

            orderItem = new()
            {
                ItemId = ItemID,
                ItemName = OrderItemName,
                ItemAmount = 1,
                ItemPrice = OrderItemPrice,
                OrderTotalPrice = OrderItemPrice
            };
            _orderItems.Add(orderItem);
        }

        // Calculate the total sum of order items
        public double TotalSum(IEnumerable<OrderItems> Items)
        {
            double total = 0;

            foreach (var item in Items)
            {
                total += item.OrderTotalPrice;
            }
            return total;
        }

        // Edit the quantity of an order item (add or subtract)
        public void EditItemAmount(List<OrderItems> _orderItems, Guid orderItemID, string action)
        {
            OrderItems orderItem = _orderItems.FirstOrDefault(x => x.ItemId == orderItemID);

            if (orderItem != null)
            {
                if (action == "add")
                {
                    orderItem.ItemAmount++;
                    orderItem.OrderTotalPrice = orderItem.ItemAmount * orderItem.ItemPrice;
                }
                else if (action == "subtract" && orderItem.ItemAmount > 1)
                {
                    orderItem.ItemAmount--;
                    orderItem.OrderTotalPrice = orderItem.ItemAmount * orderItem.ItemPrice;
                }
            }
        }

        // Delete an order item from the list
        public void DeleteFromOrderItemsList(List<OrderItems> _orderItems, Guid _orderItemID)
        {
            OrderItems orderItem = _orderItems.FirstOrDefault(x => x.ItemId == _orderItemID);

            if (orderItem != null)
            {
                _orderItems.Remove(orderItem);
            }
        }

        // Get all cafe orders from the stored file
        public List<Orders> GetAllOrders()
        {
            string orderListFilePath = Utils.GetOrdersListFilePath();

            if (!File.Exists(orderListFilePath))
            {
                return new List<Orders>();
            }

            var json = File.ReadAllText(orderListFilePath);

            return JsonSerializer.Deserialize<List<Orders>>(json);
        }

        // Add a new cafe order and save it to the file
        public void PurchaseOrder(Orders order)
        {
            List<Orders> orders = GetAllOrders();
            orders.Add(order);

            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string orderListFilePath = Utils.GetOrdersListFilePath();

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(orders);
            File.WriteAllText(orderListFilePath, json);
        }
        // Calculate discounts and redeemed coffee count for given free coffee count and order items
        public Dictionary<string, double> CoffeeCoupons(int freeCoffeeCount, List<OrderItems> Items)
        {
            int totalRedeemedCoffeeCount = 0;
            double totalDiscount = 0;

            if (Items.Count == 0 || freeCoffeeCount <= 0)
            {
                return new Dictionary<string, double>();
            }

            int TotalAmountInCart = Items.Where(item => item.ItemType == "coffee").Sum(item => item.ItemAmount);

            foreach (var item in Items)
            {
                int amountAfterFreeCoffee = Math.Max(0, item.ItemAmount - freeCoffeeCount);

                int RedeeemedAmount = amountAfterFreeCoffee == 0 ? item.ItemAmount : amountAfterFreeCoffee;

                totalRedeemedCoffeeCount += RedeeemedAmount;

                totalDiscount += (item.ItemPrice * RedeeemedAmount);

                freeCoffeeCount -= RedeeemedAmount;

                if (freeCoffeeCount <= 0)
                {
                    break;
                }
            }

            return new Dictionary<string, double>
            {
                { "discount", totalDiscount },
                { "redeemedCoffeeCount", totalRedeemedCoffeeCount}
            };
        }
    }
}
