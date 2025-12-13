using Hospital_Management_System.ViewModels;
using System.Diagnostics;

namespace Hospital_Management_System.Views;

public partial class AppointmentView : ContentPage
{

	public AppointmentView(AppointmentViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    //private void OnPointerEntered(object sender, PointerEventArgs e)
    //{
    //    Button button = (Button)sender;
    //    if (button != null)
    //    {
    //        // Modern hover effect with blue theme
    //        button.TextColor = Colors.White;
    //        button.Background = Color.FromArgb("#4A90E2");
    //        button.FontAttributes = FontAttributes.Bold;
    //        button.BorderWidth = 0;

    //        // Optional: Add subtle scale effect (requires additional setup)
    //        // button.Scale = 1.02;
    //    }
    //}

    //private void OnPointerExited(object sender, PointerEventArgs e)
    //{
    //    Button button = (Button)sender;
    //    if (button != null)
    //    {
    //        // Return to default state
    //        button.Background = Colors.Transparent;
    //        button.TextColor = Color.FromArgb("#2C3E50");
    //        button.FontAttributes = FontAttributes.None;
    //        button.BorderWidth = 0;

    //        // Optional: Reset scale
    //        // button.Scale = 1.0;
    //    }
    //    else
    //    {
    //        Debug.WriteLine("Button is null");
    //    }
    //}

}