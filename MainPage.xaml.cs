using Hospital_Management_System.ViewModels;

namespace Hospital_Management_System
{
    public partial class MainPage : ContentPage
    {
        
        public MainPage(MainViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
        

        
    }
}
