﻿using DbController;
using DbController.MySql;
using MySql.Data.MySqlClient;
using System.Text.Json;
using Tabeltop.Core.Installer;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

AppSettings settings = new AppSettings();


IDbController dbController = ConnectToDatabase();



settings.ConnectionString = dbController.ConnectionString;

bool createLocalAccount = false;

User user = new User
{
    Origin = "local",
    Salt = StringExtensions.RandomString(10)
};

do
{
    Console.WriteLine("Do you want to use local accounts? (Y/N)");
    settings.LdapSettings.ENABLE_LOCAL_LOGIN = ReadConsole(false, ValidateBoolean);
    Console.WriteLine("Do you want to use LDAP login? (Y/N)");
    settings.LdapSettings.ENABLE_LDAP_LOGIN = ReadConsole(false, ValidateBoolean);

    if (!settings.LdapSettings.ENABLE_LOCAL_LOGIN && !settings.LdapSettings.ENABLE_LDAP_LOGIN)
    {
        Console.WriteLine("You'll need to enable at least one login provider.");
    }
} while (!settings.LdapSettings.ENABLE_LOCAL_LOGIN && !settings.LdapSettings.ENABLE_LDAP_LOGIN);

if (settings.LdapSettings.ENABLE_LOCAL_LOGIN && settings.LdapSettings.ENABLE_LDAP_LOGIN)
{
    Console.WriteLine("Do you want to create a local admin account? (Y/N)");
    createLocalAccount = ReadConsole(false, ValidateBoolean);
}
else if (settings.LdapSettings.ENABLE_LOCAL_LOGIN && !settings.LdapSettings.ENABLE_LDAP_LOGIN)
{
    // When LDAP is disabled, a local account is required.
    createLocalAccount = true;
}

if (createLocalAccount)
{
    Console.WriteLine("Please enter a username:");
    user.Username = ReadConsole(false, x => ValidateString(x, false));
    Console.WriteLine("Please enter display name:");
    user.DisplayName = ReadConsole(false, x => ValidateString(x, false));
    Console.WriteLine("Please enter email:");
    user.Email = ReadConsole(false, x => ValidateString(x, false));
    Console.WriteLine("Please enter password:");
    user.Password = ReadConsole(false, x => ValidateString(x, false));


    string passwordHashed = DbInstaller.HashPassword(user);
    user.Password = passwordHashed;
}

Console.WriteLine("Creating database...");

await DbInstaller.InstallAsync(dbController);

Console.WriteLine("Loading permissions...");


if (createLocalAccount)
{
    var permissions = await PermissionService.GetAllAsync(dbController);
    user.Permissions = permissions;
    Console.WriteLine("Creating local user...");

    PermissionService permissionService = new PermissionService();
    UserService userService = new UserService(permissionService);

    await userService.CreateAsync(user, dbController);
}

Console.WriteLine("Create appsettings.json");

string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
{
    WriteIndented = true
});

await File.WriteAllTextAsync("appsettings.json", json);

Console.WriteLine("Installation has finished!");



IDbController ConnectToDatabase()
{

    IDbController? dbController = null;
    var connectionStringBuilder = new MySqlConnectionStringBuilder();
    do
    {
        Console.WriteLine("Please enter your MySQL database connection details.");
        Console.WriteLine("Hostname:");
        string hostname = ReadConsole(false, x => ValidateString(x, false));

        Console.WriteLine("Username:");
        string username = ReadConsole(false, x => ValidateString(x, false));

        Console.WriteLine("Password:");
        string password = ReadConsole(true, x => ValidateString(x, true));

        Console.WriteLine("Database:");
        string database = ReadConsole(false, x => ValidateString(x, false));

        Console.WriteLine("Port (leave empty for default 3306):");

        uint port = ReadConsole<uint>(true, x =>
        {
            if (string.IsNullOrWhiteSpace(x))
            {
                return (3306, true);
            };

            if (uint.TryParse(x, out uint port))
            {
                return (port, true);
            }
            else
            {
                return (0, false);
            }
        });


        connectionStringBuilder.Database = database;
        connectionStringBuilder.Server = hostname;
        connectionStringBuilder.UserID = username;
        connectionStringBuilder.Password = password;
        connectionStringBuilder.Port = port;
        Console.WriteLine("Connecting...");
        try
        {
            dbController = new MySqlController(connectionStringBuilder.ConnectionString);
            Console.WriteLine("Connected successfully!");
        }
        catch (Exception ex)
        {
            dbController?.Dispose();
            dbController = null;

            Console.WriteLine("Could not establish connection to database. Please try again. Error:");
            Console.WriteLine(ex);
        }


    } while (dbController is null);


    return dbController;
}

static (bool result, bool isValid) ValidateBoolean(string value)
{
    if (value.ToUpper() == "Y")
    {
        return (true, true);
    }
    else if (value.ToUpper() == "N")
    {
        return (false, true);
    }
    else
    {
        return (false, false);
    }
}
static (string result, bool isValid) ValidateString(string value, bool allowEmpty)
{

    if (string.IsNullOrWhiteSpace(value))
    {
        if (allowEmpty)
        {
            return (string.Empty, true);
        }

        return (string.Empty, false);
    }

    return (value, true);

}
static T ReadConsole<T>(bool allowEmpty, Func<string, (T result, bool isValid)> castLine)
{
    string input = string.Empty;

    while (true)
    {
        input = Console.ReadLine() ?? string.Empty;
        if (!allowEmpty && string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Please enter a valid value!");
        }

        var result = castLine(input);

        if (result.isValid)
        {
            return result.result;
        }
    }
}