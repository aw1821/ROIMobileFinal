using Microsoft.Maui.Controls;
using System;
using System.IO;
namespace ROIMobileFinal;

public partial class StaffDirectoryPage : ContentPage
{
    private Database _database;
    public StaffDirectoryPage()
	{
		InitializeComponent();
        _database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "staff.db"));
        LoadStaff();
    }
    private void LoadStaff()
    {
        var staff = _database.GetStaff();
        StaffListView.ItemsSource = staff;
    }

    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        /*if (e.SelectedItem is Staff selectedStaff)
        {
            await Navigation.PushAsync(new StaffDetailsPage(selectedStaff.StaffID));
        }*/
        if (e.SelectedItem is Staff selectedStaff)
        {
            // Deselect the item immediately to allow re-selection
            ((ListView)sender).SelectedItem = null;

            // Navigate to the StaffDetailsPage
            await Navigation.PushAsync(new StaffDetailsPage(selectedStaff.StaffID));
        }
    }
    private void OnGoToHomeClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
}