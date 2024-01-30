using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data;

// Represents the global state of the application
internal class GlobalState
{
    // The currently logged-in user
    public User CurrentUser { get; set; }
    // List of order items in the global state, initialized as an empty list
    public List<OrderItems> OrderList { get; set; } = new();
}
