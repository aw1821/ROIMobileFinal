using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ROIMobileFinal
{
    public partial class AddStaffPage : ContentPage
    {
        // Use the Database class to interact with the SQLite database
        private Database _database;

        //This constructor initializes the AddStaffPage and sets the path for the database file.
        public AddStaffPage()
        {
            InitializeComponent();
            _database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "staff.db"));
        }

        // Event handler for the Save button
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Create a new Staff object with the data entered by the user
            var newStaff = new Staff
            {
                // Assign the values from the entry fields to the properties of the Staff object
                Name = NameEntry.Text,
                Email = EmailEntry.Text,
                Mobile = MobileEntry.Text,
                Phone = PhoneEntry.Text
            };

            // Add the new staff to the database
            _database.AddStaff(newStaff);

            // Display a success message and navigate back to the Staff Directory page
            await DisplayAlert("Success", "Staff added successfully", "OK");
            await Navigation.PopAsync(); // Navigate back to the Staff Directory page
        }
    }
}
