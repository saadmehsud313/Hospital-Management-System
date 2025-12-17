using Hospital_Management_System.ViewModels;

namespace Hospital_Management_System.Views;

public partial class AdmitPatientView : ContentPage
{
	public AdmitPatientView(AdmitPatientViewModel apvm)
	{
		InitializeComponent();
		BindingContext=apvm;
    }
}