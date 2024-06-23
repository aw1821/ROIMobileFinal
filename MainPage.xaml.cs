using Microsoft.Maui.Controls;

namespace ROIMobileFinal;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    // Event handlers for the buttons on the main page
    private void OnHomeClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }

    private void OnStaffDirectoryClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StaffDirectoryPage());
    }

    private void OnAddStaffClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddStaffPage());
    }

    private void OnSettingsClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SettingsPage());
    }
}
