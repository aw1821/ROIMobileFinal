using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
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
            NameEntry.Text = staff.Name;
            EmailEntry.Text = staff.Email;
            MobileEntry.Text = staff.Mobile;
            PhoneEntry.Text = staff.Phone;
        }
    }
    private void OnEditClicked(object sender, EventArgs e)
    {
        // Enable editing
        NameEntry.IsReadOnly = false;
        EmailEntry.IsReadOnly = false;
        MobileEntry.IsReadOnly = false;
        PhoneEntry.IsReadOnly = false;

        // Show the Save button
        ((Button)sender).IsVisible = false;
        SaveButton.IsVisible = true;
    }
    private async void OnSaveClickedAsync(object sender, EventArgs e)
    {
        // Update the staff details in the database
        var staff = new Staff
        {
            StaffID = _staffID,
            Name = NameEntry.Text,
            Email = EmailEntry.Text,
            Mobile = MobileEntry.Text,
            Phone = PhoneEntry.Text
        };

        _database.UpdateStaff(staff);

        // Disable editing
        NameEntry.IsReadOnly = true;
        EmailEntry.IsReadOnly = true;
        MobileEntry.IsReadOnly = true;
        PhoneEntry.IsReadOnly = true;

        // Show the Edit button
        ((Button)sender).IsVisible = false;
        EditButton.IsVisible = true;

        PopupMessageFrame.IsVisible = true;
        await Task.Delay(2000);
        PopupMessageFrame.IsVisible = false;
    }
    private void OnGoToHomeClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
    private void OnGoToStaffDirectoryClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StaffDirectoryPage());
    }
    private async void OnDeleteStaffClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this staff?", "Yes", "No");
        if (confirm)
        {
            _database.DeleteStaff(_staffID);
            await DisplayAlert("Deleted", "Staff deleted successfully", "OK");
            await Navigation.PopAsync(); // Navigate back to the Staff Directory page
        }
    }

}