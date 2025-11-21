using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hospital_Management_System.ViewModels;

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
                button.BorderColor = Color.FromArgb("#EC9C13");
                button.BorderWidth = 1;
                button.Background = Colors.White;
                button.FontAttributes = FontAttributes.Bold;
            }
        }
        private void OnPointerExited(object sender,PointerEventArgs e)
        {
            Button button = (Button) sender;
            if (button != null)
            {
                button.Background = Color.FromArgb("#EFA30B");
                button.TextColor = Colors.White;
                button.FontAttributes = FontAttributes.None;
            }
        }

        
    }
}