using Hospital_Management_System.ViewModels;

namespace Hospital_Management_System.Views;

public partial class DoctorPage : ContentPage
{
	private readonly DoctorViewModel _viewModel;
    public DoctorPage(DoctorViewModel model)
	{
		InitializeComponent();
		BindingContext= _viewModel =model;
    }
	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.OnAppearing();
    }
}