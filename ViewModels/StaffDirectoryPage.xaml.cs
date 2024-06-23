using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace ROIMobileFinal;

public partial class StaffDirectoryPage : ContentPage
{
    // Declare the database and list of staff
    private Database _database;
    private List<Staff> _allStaff;

    // Initialize the StaffDirectoryPage and set the path for the database file
    public StaffDirectoryPage()
    {
        InitializeComponent();
        _database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "staff.db"));
        LoadStaff();
    }
    // Load the staff list from the database
    private void LoadStaff()
    {
        // Get all staff from the database using the GetStaff method from the Database class
        _allStaff = _database.GetStaff();
        // Set the data source for StaffListView to display all staff from the database
        StaffListView.ItemsSource = _allStaff;
    }

    // Event handlers for the buttons on the StaffDirectoryPage
    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Staff selectedStaff)
        {
            // Deselect the item immediately to allow re-selection
            ((ListView)sender).SelectedItem = null;

            // Navigate to the StaffDetailsPage
            await Navigation.PushAsync(new StaffDetailsPage(selectedStaff.StaffID));
        }
    }

    private async void OnAddStaffClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddStaffPage());
    }

    private void OnGoToHomeClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }

    // Event handler for the Delete Database button
    private void OnDeleteDatabaseClicked(object sender, EventArgs e)
    {
        var database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "staff.db"));
        // Delete the database file using the DeleteDatabase method from the Database class
        database.DeleteDatabase();
    }
    // Event handler for the Refresh button
    protected override void OnAppearing()
    {
        base.OnAppearing();
        //Reload the staff list when the page appears
        LoadStaff(); 
    }
    private void OnSettingsClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SettingsPage());
    }
    // Event handler for the SearchBar text changed event
    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        //Store the text from search bar in a variable
        string filter = e.NewTextValue;
        //Check if the search bar text is empty or null
        if (string.IsNullOrWhiteSpace(filter))
        {
            //If search bar text is empty or null, display all staff
            StaffListView.ItemsSource = _allStaff;
        }
        else
        {
            //If there is text in the search bar, filter the staff list based on the search text
            //Uses case-insensitive search to filter staff by name or email by converting search text and staff data to lowercase
            StaffListView.ItemsSource = _allStaff.Where(staff => staff.Name.ToLower().Contains(filter.ToLower()) || staff.Email.ToLower().Contains(filter.ToLower())).ToList();
        }
    }

}
