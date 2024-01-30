using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    // Represents a coffee item in a cafe menu
    public class CoffeeItems
    {
        // Unique identifier for the coffee item
        public Guid CoffeeId { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Please provide the coffee type.")]
        // The type or name of the coffee, required with validation error message
        public string CoffeeType { get; set; }
        // The price of the coffee item
        public double Price { get; set; }
    }
}
