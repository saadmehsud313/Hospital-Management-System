using Hospital_Management_System.ViewModels;

namespace Hospital_Management_System.Views;

public partial class VisitPage : ContentPage
{
	public VisitPage(VisitManagementViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}