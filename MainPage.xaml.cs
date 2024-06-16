using Microsoft.Maui.Controls;
using System;
namespace ROIMobileFinal
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnGoToStaffDirectoryClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StaffDirectoryPage());
        }
    }

}
