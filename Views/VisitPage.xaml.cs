using Hospital_Management_System.ViewModels;

namespace Hospital_Management_System.Views;

public partial class VisitPage : ContentPage
{
	private readonly VisitManagementViewModel _viewModel;
    public VisitPage(VisitManagementViewModel vm)
	{
		InitializeComponent();
		BindingContext = _viewModel = vm;
    }

	protected override void OnAppearing()
	{
		base.OnAppearing();
		//_viewModel.OnAppearing();
    }

}