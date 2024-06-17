using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ROIMobileFinal
{
    public partial class AddStaffPage : ContentPage
    {
        private Database _database;

        public AddStaffPage()
        {
            InitializeComponent();
            _database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "staff.db"));
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var newStaff = new Staff
            {
                Name = NameEntry.Text,
                Email = EmailEntry.Text,
                Mobile = MobileEntry.Text,
                Phone = PhoneEntry.Text
            };

            _database.AddStaff(newStaff);

            await DisplayAlert("Success", "Staff added successfully", "OK");
            await Navigation.PopAsync(); // Navigate back to the Staff Directory page
        }
    }
}
