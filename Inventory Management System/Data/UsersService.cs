using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Data;

namespace Inventory_Management_System.Data;

public static class UsersService
{
    public const string SeedUsername = "admin";
    public const string SeedPassword = "admin";


    private static void SaveAll(List<User> staffs)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appStaffFilePath = Utils.GetAppUserFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(staffs);
        File.WriteAllText(appStaffFilePath, json);
    }
    public static List<User> GetAll()
    {
        string appStaffFilePath = Utils.GetAppUserFilePath();
        if (!File.Exists(appStaffFilePath))
        {
            return new List<User>();
        }

        var json = File.ReadAllText(appStaffFilePath);

        return JsonSerializer.Deserialize<List<User>>(json);
    }

    public static List<User> Create(Guid staffId, string username, string firstname, string lastname, string email, string password, Role role)
    {
        List<User> users = GetAll();
        bool usernameExists = users.Any(x => x.Username == username);

        int totalAdmin = users.Where(x => x.Role == Role.Admin).Count();


        if (usernameExists)
        {
            throw new Exception("Username already exists.");
        }

        if (totalAdmin >=2 && role == Role.Admin)
        {
            throw new Exception("Two admins already exists");
        }
        users.Add(
            new User
            {
                Username = username,
                Firstname = firstname,
                Lastname = lastname,
                Email = email,
                PasswordHash = Utils.HashSecret(password),
                Role = role,
                CreatedBy = staffId
            }
        );
        SaveAll(users);
        return users;
    }


    public static void SeedUsers()
    {
        var users = GetAll().FirstOrDefault(x => x.Role == Role.Admin);

        if (users == null)
        {
            Create(Guid.Empty, SeedUsername, SeedPassword, Role.Admin);
        }
    }

    private static void Create(Guid empty, string seedUsername, string seedPassword, Role admin)
    {
        throw new NotImplementedException();
    }

    public static User GetById(Guid id)
    {
        List<User> users = GetAll();
        return users.FirstOrDefault(x => x.Id == id);
    }
    public static List<User> Delete(Guid id)
    {
        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            throw new Exception("Staff not found.");
        }

        users.Remove(user);
        SaveAll(users);

        return users;
    }
    public static User Login(string username, string password)
    {
        var loginErrorMessage = "Invalid username or password.";
        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Username == username);

        if (user == null)
        {
            throw new Exception(loginErrorMessage);
        }

        bool passwordIsValid = Utils.VerifyHash(password, user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new Exception(loginErrorMessage);
        }

        return user;
    }

      public static User ChangePassword(Guid id, string currentPassword, string newPassword)
    {
        if (currentPassword == newPassword)
        {
            throw new Exception("New password must be different from current password.");
        }

        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            throw new Exception("Staff not found.");
        }

        bool passwordIsValid = Utils.VerifyHash(currentPassword, user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new Exception("Incorrect current password.");
        }

        user.PasswordHash = Utils.HashSecret(newPassword);
        user.HasInitialPassword = false;
        SaveAll(users);

        return user;
    }

}
