using Hospital_Management_System.Views;
namespace Hospital_Management_System
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));





        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var loginPage = MauiProgram.Services.GetService<LoginPage>();
            return new Window(loginPage);
        }
    }
}