using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
namespace ROIMobileFinal;

public partial class StaffDetailsPage : ContentPage
{
    // Define Variables, Database and StaffID using the Database class to interact with the SQLite database
    private Database _database;
    private int _staffID;
    
    //This is the constructor for the StaffDetailsPage class. It initializes the page with the staff details based on the staffID provided.
    public StaffDetailsPage(int staffID)
	{
		InitializeComponent();
        _staffID = staffID;
        _database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "staff.db"));
        Debug.WriteLine($"This is the pathway to the database: {_database.GetDatabasePath()}");
        LoadStaffDetails();
    }
    //This method loads the staff details from the database and populates the entry fields on the page.
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
    // Event handlers for the buttons on the StaffDetailsPage
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
    //This method saves the updated staff details to the database and disables editing.
    /*
     * One note is i haven't implemented validation for fields , so the user can save empty fields it will give an error in the database.
     * In the mean time just assume all fields have to be filled out.
     */
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
        // Show a popup message to indicate that the staff details have been saved
        PopupMessageFrame.IsVisible = true;
        await Task.Delay(2000);
        PopupMessageFrame.IsVisible = false;
    }
    //This method deletes the staff from the database and navigates back to the Staff Directory page.
    private async void OnDeleteStaffClicked(object sender, EventArgs e)
    {
        // Show a confirmation dialog before deleting the staff
        bool confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this staff?", "Yes", "No");
        if (confirm)
        {
            // Delete the staff from the database
            _database.DeleteStaff(_staffID);
            await DisplayAlert("Deleted", "Staff deleted successfully", "OK");
            // Navigate back to the Staff Directory page
            await Navigation.PopAsync(); 
        }
    }
    // Event handlers for the buttons on the StaffDetailsPage
    private void OnGoToHomeClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
    private void OnStaffDirectoryClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StaffDirectoryPage());
    }
    private void OnSettingsClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SettingsPage());
    }
    

}