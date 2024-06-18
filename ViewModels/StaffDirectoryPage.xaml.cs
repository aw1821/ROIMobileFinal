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
    private Database _database;
    private List<Staff> _allStaff;

    public StaffDirectoryPage()
    {
        InitializeComponent();
        _database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "staff.db"));
        LoadStaff();
    }

    private void LoadStaff()
    {
        _allStaff = _database.GetStaff();
        StaffListView.ItemsSource = _allStaff;
    }

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

    private void OnDeleteDatabaseClicked(object sender, EventArgs e)
    {
        var database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "staff.db"));
        database.DeleteDatabase();
    }

    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        string filter = e.NewTextValue;
        if (string.IsNullOrWhiteSpace(filter))
        {
            StaffListView.ItemsSource = _allStaff;
        }
        else
        {
            StaffListView.ItemsSource = _allStaff
                .Where(staff => staff.Name.ToLower().Contains(filter.ToLower()) || staff.Email.ToLower().Contains(filter.ToLower()))
                .ToList();
        }
    }
}
