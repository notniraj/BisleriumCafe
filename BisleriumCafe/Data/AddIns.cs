using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    // Represents an Add-In entity in a cafe application
    public class AddIns
    {
        // Unique identifier for each Add-In, initialized with a new GUID
        public Guid Id { get; set; } = Guid.NewGuid();

        // Type of the Add-In, required field with validation error message
        [Required(ErrorMessage = "Please provide the AddIn type.")]
        public string AddInType { get; set; }
        // Price of the Add-In
        public double Price { get; set; }

    }
}
