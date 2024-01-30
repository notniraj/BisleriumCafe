using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace BisleriumCafe.Data
{
    // Service class for managing coffee items in a cafe
    public class CoffeeServices
    {
        // Save all coffee items to a file
        private static void SaveAll(List<CoffeeItems> coffeeList)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string coffeeListFilePath = Utils.GetCoffeesFilePath();

            // Create the app data directory if it doesn't exist
            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            // Serialize the list of coffee items to JSON and write to file
            var json = JsonSerializer.Serialize(coffeeList);
            File.WriteAllText(coffeeListFilePath, json);
        }

        // Get a coffee item by its unique identifier
        public static CoffeeItems GetCofeeByID(String cID)
        {
            List<CoffeeItems> coffeeList = GetAll();
            CoffeeItems coffee = coffeeList.FirstOrDefault(coffee => coffee.CoffeeId.ToString() == cID);
            return coffee;
        }

        // Initial coffee list with default items
        private readonly List<CoffeeItems> _coffeeList = new()
        {
            
            new() { CoffeeType = "Americano", Price = 140 },
            new() { CoffeeType = "Latte", Price = 170},
            new() { CoffeeType = "Mocha", Price = 180},
            new() { CoffeeType = "Macchiato", Price = 160},
            new() { CoffeeType = "Cappuccino", Price = 150},
            new() { CoffeeType = "Espresso", Price = 120},

        };

        /* 
         * Seed method to populate the initial coffee list if it's empty
         * Uncomment if needed
         */
        /* public void SeedCofeeDetails()
        {
            List<CoffeeItems> coffeeList = GetAll();

            if (coffeeList.Count == 0)
            {
                SaveAll(_coffeeList);
            }
        }*/

        // Create a new coffee item
        public static List<CoffeeItems> CreateCoffee(Guid userId, string coffeeType, double price)
        {
            if (coffeeType == "")
            {
                throw new Exception("Coffee Info is required");
            }

            // Validate that the price is greater than zero
            if (price <= 0)
            {
                throw new Exception("Price must be over $0");
            }

            // Get the current list of coffee items
            List<CoffeeItems> coffees = GetAll();
            // Add the new coffee item to the list
            coffees.Add(new CoffeeItems
            {
                CoffeeType = coffeeType,
                Price = price
            });
            // Save the updated list
            SaveAll(coffees);
            // Return the updated list
            return coffees;
        }

        // Get all coffee items from the file
        public static List<CoffeeItems> GetAll()
        {
            string CoffeesFilePath = Utils.GetCoffeesFilePath();
            // If the file doesn't exist, return an empty list
            if (!File.Exists(CoffeesFilePath))
            {
                return new List<CoffeeItems>();
            }

            // Read the JSON content from the file and deserialize it to a list of coffee items
            var json = File.ReadAllText(CoffeesFilePath);

            return JsonSerializer.Deserialize<List<CoffeeItems>>(json);
        }

        // Delete a coffee item from the list
        public static List<CoffeeItems> DelCoffeeInList(Guid cID)
        {
            // Get the current list of coffee items
            List<CoffeeItems> coffeeList = GetAll();

            // Find the coffee item to delete by its ID
            CoffeeItems coffee = coffeeList.FirstOrDefault(coffee => coffee.CoffeeId.ToString() == cID.ToString());

            // If the coffee item exists, remove it from the list and save the updated list
            if (coffee != null)
            {
                coffeeList.Remove(coffee);
                SaveAll(coffeeList);
            }
            // Return the updated list
            return coffeeList;
        }

        // Update information for a coffee item
        public static void Update(CoffeeItems coffee)
        {
            // Get the current list of coffee items
            List<CoffeeItems> coffeeList = GetAll();

            // Find the coffee item to update by its ID
            CoffeeItems toEditCoffee = coffeeList.FirstOrDefault(_coffee => _coffee.CoffeeId.ToString() == coffee.CoffeeId.ToString());

            // If the coffee item is found, update its properties and save the updated list
            if (toEditCoffee == null)
            {
                throw new Exception("Coffee not found");
            }

            toEditCoffee.CoffeeType = coffee.CoffeeType;
            toEditCoffee.Price = coffee.Price;

            SaveAll(coffeeList);
        }
    }
}
