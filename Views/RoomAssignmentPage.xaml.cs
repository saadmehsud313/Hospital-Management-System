using Hospital_Management_System.ViewModels;

namespace Hospital_Management_System.Views;

public partial class RoomAssignmentPage : ContentPage
{
	public RoomAssignmentPage(RoomAssignmentViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}