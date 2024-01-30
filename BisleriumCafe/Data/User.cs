using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    // Represents a user in the application
    public class User
    {
        // Unique identifier for the user
        public Guid Id { get; set; } = Guid.NewGuid();
        // User's username
        public string Username { get; set; }
        // Hashed password for user authentication
        public string PasswordHash { get; set; }
        // Role of the user (e.g., User or Admin)
        public Role Role { get; set; }
        // Date and time when the user was created
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // Unique identifier of the user who created this user
        public Guid CreatedBy { get; set; }
    }
}
