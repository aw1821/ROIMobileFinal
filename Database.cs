using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;

// Database class to handle SQLite operations
public class Database
{
    // Path to the SQLite database file
    private readonly string _databasePath;
    // Method to get the database path
    public string GetDatabasePath()
    {
        return _databasePath;
    }
    //Method to create a new instance of the Database class
    public Database(string databasePath)
    {
        _databasePath = databasePath;
        InitializeDatabase();
    }

    //Method to initialize the database with the Staff table and initial data
    private void InitializeDatabase()
    {
        // Create the Staff table if it doesn't exist and open a connection to the database
        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();

        // Create the Staff table if it doesn't exist
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS Staff (
                                    StaffID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name NVARCHAR(50),
                                    Email NVARCHAR(50),
                                    Mobile INTEGER(10),
                                    Phone INTEGER(10)
                                );";
        // Execute the command to create the table
        tableCmd.ExecuteNonQuery();

        // Check if initial data exists
        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = "SELECT COUNT(*) FROM Staff;";
        // Execute the command to check the count of records in the Staff table
        var count = (long)checkCmd.ExecuteScalar();

        // Insert initial data if needed if the Staff table is empty
        if (count == 0)
        {
            // Insert initial data if needed
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = @"INSERT INTO Staff (Name, Email, Mobile, Phone) 
                                      VALUES ('Will Smith', 'Will@gmail.com', '1234567890', '0987654321'),
                                             ('Jane Smith', 'jane@icloud.com', '2345678901', '9876543210');";
            // Insert the initial data into the Staff table
            insertCmd.ExecuteNonQuery();
        }
    }

    //Method to get all staff from the database
    public List<Staff> GetStaff()
    {
        // Create a list to store the staff data
        var staffList = new List<Staff>();

        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();

        // Create a command to select all staff from the Staff table and execute the command
        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT StaffID, Name, Email FROM Staff;";
        using var reader = selectCmd.ExecuteReader();
        // Read the data from the reader and add it to the staff list
        while (reader.Read())
        {
            staffList.Add(new Staff
            {
                StaffID = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2)
            });
        }
        // Return the list of staff
        return staffList;
    }

    //Method to get the details of a specific staff member based on the staffID
    //Staff ID is the primary key of the Staff table, hidden from the user but used to identify staff members in the backend
    public Staff GetStaffDetails(int staffID)
    {
        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();

        var selectCmd = connection.CreateCommand();
        // SQL command to select a specific staff member based on the staffID
        selectCmd.CommandText = $"SELECT * FROM Staff WHERE StaffID = {staffID};";
        using var reader = selectCmd.ExecuteReader();

        // Read the data from the reader and return the staff member details
        if (reader.Read())
        {
            return new Staff
            {
                StaffID = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Mobile = reader.GetString(3),
                Phone = reader.GetString(4)
            };
        }
        // Return null if no staff member is found by the staffID
        return null;
    }

    //Method to add a new staff member to the database
    public void AddStaff(Staff staff)
    {
        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();
        // Create a command to insert a new staff member into the Staff table
        var insertCmd = connection.CreateCommand();
        insertCmd.CommandText = @"INSERT INTO Staff (Name, Email, Mobile, Phone) 
                                  VALUES (@Name, @Email, @Mobile, @Phone);";
        insertCmd.Parameters.AddWithValue("@Name", staff.Name);
        insertCmd.Parameters.AddWithValue("@Email", staff.Email);
        insertCmd.Parameters.AddWithValue("@Mobile", staff.Mobile);
        insertCmd.Parameters.AddWithValue("@Phone", staff.Phone);
        insertCmd.ExecuteNonQuery();
    }

    //Method to delete a staff member from the database based on the staffID
    public void DeleteStaff(int staffID)
    {
        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();
        // Create a command to delete a staff member from the Staff table based on the staffID
        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM Staff WHERE StaffID = @StaffID;";
        deleteCmd.Parameters.AddWithValue("@StaffID", staffID);
        deleteCmd.ExecuteNonQuery();
    }
    //Method to update the details of a staff member in the database
    public void UpdateStaff(Staff staff)
    {
        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();
        // Create a command to update the details of a staff member in the Staff table based on the staffID
        var updateCmd = connection.CreateCommand();
        updateCmd.CommandText = @"UPDATE Staff 
                              SET Name = @Name, Email = @Email, Mobile = @Mobile, Phone = @Phone 
                              WHERE StaffID = @StaffID;";
        updateCmd.Parameters.AddWithValue("@Name", staff.Name);
        updateCmd.Parameters.AddWithValue("@Email", staff.Email);
        updateCmd.Parameters.AddWithValue("@Mobile", staff.Mobile);
        updateCmd.Parameters.AddWithValue("@Phone", staff.Phone);
        updateCmd.Parameters.AddWithValue("@StaffID", staff.StaffID);
        updateCmd.ExecuteNonQuery();
    }
}
// Staff class to represent a staff member
public class Staff
{
    // Properties of the Staff class, getters and setters for the staff details
    public int StaffID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string Phone { get; set; }
}
