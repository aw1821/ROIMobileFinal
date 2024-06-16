using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Diagnostics;
namespace ROIMobileFinal;

public partial class StaffDetailsPage : ContentPage
{
    private Database _database;
    private int _staffID;
    public StaffDetailsPage(int staffID)
	{
		InitializeComponent();
        _staffID = staffID;
        _database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "staff.db"));
        Debug.WriteLine($"This is the pathway to the database: {_database.GetDatabasePath()}");
        LoadStaffDetails();
    }
    private void LoadStaffDetails()
    {
        var staff = _database.GetStaffDetails(_staffID);

        if (staff != null)
        {
            NameLabel.Text = staff.Name;
            EmailLabel.Text = staff.Email;
            MobileLabel.Text = staff.Mobile;
            PhoneLabel.Text = staff.Phone;
        }
    }
}