namespace ROIMobileFinal
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {   
            // Register the routes for the pages
            InitializeComponent();
            Routing.RegisterRoute(nameof(StaffDirectoryPage), typeof(StaffDirectoryPage));
            Routing.RegisterRoute(nameof(StaffDetailsPage), typeof(StaffDetailsPage));
            Routing.RegisterRoute(nameof(AddStaffPage), typeof(AddStaffPage));
        }
    }
}
