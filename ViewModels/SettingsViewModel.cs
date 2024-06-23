using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;

namespace ROIMobileFinal.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        //manage the settings of the application. In this case, the font size of the application
        public SettingsViewModel()
        {
            FontSize = Preferences.Get("FontSize", 14);
        }

        // Font size property using the ObservableProperty attribute from the CommunityToolkit.Mvvm.ComponentModel namespace
        [ObservableProperty]
        private int fontSize;

        // RelayCommand attribute from the CommunityToolkit.Mvvm.Input namespace to create a command that saves the font size setting
        [RelayCommand]
        private void SaveSettings()
        {
            Preferences.Set("FontSize", FontSize);
            // Call the ApplyFontSize method to apply the font size setting to the application
            ApplyFontSize();
        }
        // Function to apply the font size setting to the application, which is called when the font size setting is saved
        private void ApplyFontSize()
        {
            Application.Current.Resources["GlobalFontSize"] = FontSize;
        }
    }
}

