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
        private void ChangePasswordClicked(object sender,EventArgs e)
        {
            Button button = (Button)sender;
            PersonalInfoPage.IsVisible = false;
            LoginInfoPage.IsVisible = true;
            button.TextColor = Color.FromArgb("#EC9C13");
            button.BorderColor = Color.FromArgb("#EC9C13");
            button.Background = Colors.White;
        }
        private void ProfilePageClicked(object sender, EventArgs e)
        {
            PersonalInfoPage.IsVisible = true;
            LoginInfoPage.IsVisible = false;
        }
        private void OnPointerEntered(object sender,PointerEventArgs e)
        {
            Button button = (Button) sender;
            if (button != null)
            {
                button.TextColor = Color.FromArgb("#EC9C13");
                button.BackgroundColor = Colors.White;
                button.FontAttributes = FontAttributes.Bold;
            }
        }
        private void OnPointerExited(object sender,PointerEventArgs e)
        {
            Button button = (Button) sender;
            if (button != null)
            {
                button.BackgroundColor = Colors.AntiqueWhite;
                button.TextColor = Color.FromArgb("#EC9133");
                button.FontAttributes = FontAttributes.None;
            }
            else
            {
                Debug.WriteLine("Button is null");
            }
        }

        
    }
}