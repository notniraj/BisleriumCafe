using System.Collections.Generic;
using System.Data;
using System.Text.Json;

namespace BisleriumCafe.Data;

// Service for managing user-related operations
public class UsersService
{
    // Constants for seeding admin user credentials
    public const string SeedUsername = "admin";
    public const string SeedPassword = "admin";

    // Save all users to the file
    public static void SaveAll(List<User> users)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appUsersFilePath = Utils.GetAppUsersFilePath();

        // Save all users to the file
        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        // Serialize users to JSON and write to file
        var json = JsonSerializer.Serialize(users);
        File.WriteAllText(appUsersFilePath, json);
    }

    // Retrieve all users from the file
    public static List<User> GetAll()
    {
        string appUsersFilePath = Utils.GetAppUsersFilePath();
        // If the file doesn't exist, return an empty list
        if (!File.Exists(appUsersFilePath))
        {
            return new List<User>();
        }

        // Deserialize users from JSON
        var json = File.ReadAllText(appUsersFilePath);

        return JsonSerializer.Deserialize<List<User>>(json);
    }

    // Create a new user
    public static List<User> Create(Guid userId, string username, string password, Role role)
    {
        List<User> users = GetAll();
        // Check if the username already exists
        bool usernameExists = users.Any(x => x.Username == username);

        if (usernameExists)
        {
            throw new Exception("Username already exists.");
        }

        users.Add(
            new User
            {
                Username = username,
                PasswordHash = Utils.HashSecret(password),
                Role = role,
                CreatedBy = userId
            }
        );
        // Save the updated user list
        SaveAll(users);
        return users;
    }

    // Seed the admin user if it doesn't exist
    public static void SeedUsers()
    {
        var users = GetAll().FirstOrDefault(x => x.Role == Role.Admin);

        if (users == null)
        {
            Create(Guid.Empty, SeedUsername, SeedPassword, Role.Admin);
        }
    }

    // Get a user by their ID
    public static User GetById(Guid id)
    {
        List<User> users = GetAll();
        return users.FirstOrDefault(x => x.Id == id);
    }

    // Delete a user by their ID
    public static List<User> Delete(Guid id)
    {
        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        // Remove the user from the list
        users.Remove(user);
        // Save the updated user list
        SaveAll(users);

        return users;
    }

    // Validate user login credentials
    public static User Login(string username, string password)
    {
        var loginErrorMessage = "Invalid username or password.";
        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Username == username);

        if (user == null)
        {
            throw new Exception(loginErrorMessage);
        }

        // Verify the password using the hashed version stored in the user object
        bool passwordIsValid = Utils.VerifyHash(password, user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new Exception(loginErrorMessage);
        }

        return user;
    }

    // Change the password for a user
    public static void ChangePassword(Guid id, string currentPassword, string newPassword)
    {
        if (currentPassword == newPassword)
        {
            throw new Exception("New password must be different from current password.");
        }

        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        // Verify the current password before updating
        bool passwordIsValid = Utils.VerifyHash(currentPassword, user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new Exception("Incorrect current password.");
        }

        // Update the password with the hashed version of the new password
        user.PasswordHash = Utils.HashSecret(newPassword);
        // Save the updated user list
        SaveAll(users);
    }
}