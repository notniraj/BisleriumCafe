using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    // Service class for managing Add-Ins in a cafe application
    public class AddinServices
    {


        /*public void SeedAddInsList()
        {
            List<AddIns> addInsList = GetAllAddIns();

            if (addInsList.Count == 0)
            {
                SaveAllAddIns(_addInsList);
            }
        }*/

        // Saves the list of Add-Ins to a JSON file
        private static void SaveAllAddIns(List<AddIns> addInsList)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string addInsListFilePath = Utils.GetAddinsListFilePath();

            // Ensure the directory exists
            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            // Serialize the list to JSON and write to file
            var json = JsonSerializer.Serialize(addInsList);
            File.WriteAllText(addInsListFilePath, json);
        }

        // Retrieves the list of all Add-Ins from the JSON file
        public static List<AddIns> GetAllAddIns()
        {
            string addInsListFilePath = Utils.GetAddinsListFilePath();
            // Check if the file exists
            if (!File.Exists(addInsListFilePath))
            {
                return new List<AddIns>();
            }

            // Read JSON from file and deserialize into a list of Add-Ins
            var json = File.ReadAllText(addInsListFilePath);

            return JsonSerializer.Deserialize<List<AddIns>>(json);
        }

        // Retrieves an Add-In by its unique identifier
        public static AddIns GetAddInById(String addInId)
        {
            List<AddIns> addInsList = GetAllAddIns();
            // Find the Add-In with the specified ID
            AddIns addIn = addInsList.FirstOrDefault(coffee => coffee.Id.ToString() == addInId);
            return addIn;
        }

        // Creates a new Add-In with the provided name and price
        public void CreateNewAddIn(string addInName, double price)
        {
            if (addInName == "")
            {
                throw new Exception("Addin Info Required");
            }

            if (price <= 0)
            {
                throw new Exception("Price cannot be zero or below");
            }
            AddIns addIns = new()
            {
                AddInType = addInName,
                Price = price
            };

            List<AddIns> addInsList = GetAllAddIns();
            addInsList.Add(addIns);
            SaveAllAddIns(addInsList);
        }

        // Updates the information of an existing Add-In
        public void UpdateAddInInfo(AddIns addIn)
        {
            List<AddIns> addInList = GetAllAddIns();

            // Find the Add-In to be updated
            AddIns addInPick = addInList.FirstOrDefault(addInPick => addInPick.Id.ToString() == addIn.Id.ToString());

            if (addInPick == null)
            {
                throw new Exception("Add-In item not found");
            }

            // Update the Add-In's information
            addInPick.AddInType = addIn.AddInType;
            addInPick.Price = addIn.Price;

            // Save the updated list
            SaveAllAddIns(addInList);
        }


        // Deletes an Add-In from the list by its unique identifier
        public List<AddIns> DeletAddIn(Guid addInID)
        {
            List<AddIns> addInList = GetAllAddIns();
            // Find the Add-In to be deleted
            AddIns addInItem = addInList.FirstOrDefault(addIn => addIn.Id.ToString() == addInID.ToString());

            if (addInItem != null)
            {
                // Remove the Add-In and save the updated list
                addInList.Remove(addInItem);
                SaveAllAddIns(addInList);
            }

            return addInList;
        }
    }
}
