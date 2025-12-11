using Hospital_Management_System.ViewModels;

namespace Hospital_Management_System.Views;

public partial class DoctorPage : ContentPage
{
	public DoctorPage(DoctorViewModel model)
	{
		InitializeComponent();
		BindingContext=model;
    }
}