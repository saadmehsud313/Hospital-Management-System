using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hospital_Management_System.ViewModels;
using System.Diagnostics;

namespace Hospital_Management_System.Views
{
public partial class ReceptionistView : ContentPage
{
	public ReceptionistView(ReceptionistViewModel models)
	{
        InitializeComponent();
        BindingContext = models;
    }
        private void ChangePasswordClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            PersonalInfoPage.IsVisible = false;
            LoginInfoPage.IsVisible = true;

            // Update to modern blue theme
            button.TextColor = Color.FromArgb("#4A90E2");
            button.BorderColor = Color.FromArgb("#4A90E2");
            button.Background = Colors.White;
            button.BorderWidth = 2;
        }

        private void ProfilePageClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            PersonalInfoPage.IsVisible = true;
            LoginInfoPage.IsVisible = false;

            // Update to modern blue theme
            button.TextColor = Color.FromArgb("#4A90E2");
            button.BorderColor = Color.FromArgb("#4A90E2");
            button.Background = Colors.White;
            button.BorderWidth = 2;
        }

        private void OnPointerEntered(object sender, PointerEventArgs e)
        {
            Button button = (Button)sender;
            if (button != null)
            {
                // Modern hover effect with blue theme
                button.TextColor = Colors.White;
                button.BackgroundColor = Color.FromArgb("#4A90E2");
                button.FontAttributes = FontAttributes.Bold;
                button.BorderWidth = 0;

                // Optional: Add subtle scale effect (requires additional setup)
                // button.Scale = 1.02;
            }
        }

        private void OnPointerExited(object sender, PointerEventArgs e)
        {
            Button button = (Button)sender;
            if (button != null)
            {
                // Return to default state
                button.BackgroundColor = Colors.Transparent;
                button.TextColor = Color.FromArgb("#2C3E50");
                button.FontAttributes = FontAttributes.None;
                button.BorderWidth = 0;

                // Optional: Reset scale
                // button.Scale = 1.0;
            }
            else
            {
                Debug.WriteLine("Button is null");
            }
        }

    }
}