using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;

public class Database
{
    private readonly string _databasePath;
    public string GetDatabasePath()
    {
        return _databasePath;
    }
    public Database(string databasePath)
    {
        _databasePath = databasePath;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS Staff (
                                    StaffID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name NVARCHAR(50),
                                    Email NVARCHAR(50),
                                    Mobile INTEGER(10),
                                    Phone INTEGER(10)
                                );";
        tableCmd.ExecuteNonQuery();

        // Check if initial data exists
        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = "SELECT COUNT(*) FROM Staff;";
        var count = (long)checkCmd.ExecuteScalar();

        if (count == 0)
        {
            // Insert initial data if needed
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = @"INSERT INTO Staff (Name, Email, Mobile, Phone) 
                                      VALUES ('John Doe', 'john@example.com', '1234567890', '0987654321'),
                                             ('Jane Smith', 'jane@example.com', '2345678901', '9876543210');";
            insertCmd.ExecuteNonQuery();
        }
    }

    public List<Staff> GetStaff()
    {
        var staffList = new List<Staff>();

        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT StaffID, Name, Email FROM Staff;";
        using var reader = selectCmd.ExecuteReader();

        while (reader.Read())
        {
            staffList.Add(new Staff
            {
                StaffID = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2)
            });
        }

        return staffList;
    }

    public Staff GetStaffDetails(int staffID)
    {
        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = $"SELECT * FROM Staff WHERE StaffID = {staffID};";
        using var reader = selectCmd.ExecuteReader();

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

        return null;
    }

    public void AddStaff(Staff staff)
    {
        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();

        var insertCmd = connection.CreateCommand();
        insertCmd.CommandText = @"INSERT INTO Staff (Name, Email, Mobile, Phone) 
                                  VALUES (@Name, @Email, @Mobile, @Phone);";
        insertCmd.Parameters.AddWithValue("@Name", staff.Name);
        insertCmd.Parameters.AddWithValue("@Email", staff.Email);
        insertCmd.Parameters.AddWithValue("@Mobile", staff.Mobile);
        insertCmd.Parameters.AddWithValue("@Phone", staff.Phone);
        insertCmd.ExecuteNonQuery();
    }

    public void DeleteStaff(int staffID)
    {
        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM Staff WHERE StaffID = @StaffID;";
        deleteCmd.Parameters.AddWithValue("@StaffID", staffID);
        deleteCmd.ExecuteNonQuery();
    }

    public void UpdateStaff(Staff staff)
    {
        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();

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

    public void DeleteDatabase()
    {
        if (File.Exists(_databasePath))
        {
            File.Delete(_databasePath);
            Debug.WriteLine("Database deleted successfully.");
        }
        else
        {
            Debug.WriteLine("Database file not found.");
        }
    }

}

public class Staff
{
    public int StaffID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string Phone { get; set; }
}
