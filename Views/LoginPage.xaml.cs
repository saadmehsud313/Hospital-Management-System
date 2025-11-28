using Hospital_Management_System.ViewModels;
namespace Hospital_Management_System.Views
{
	public partial class LoginPage : ContentPage
	{
		
        public LoginPage(LoginViewModel model)
		{
			InitializeComponent();
			BindingContext = model;
        }
        private void OnPointerEntered(object sender, PointerEventArgs e)
        {
            Button button = (Button)sender;
            if (button != null)
            {
                button.TextColor = Color.FromArgb("#EC9C13");
                button.BorderColor = Color.FromArgb("#EC9C13");
                button.Background = Colors.White;
                button.FontAttributes = FontAttributes.Bold;
            }
        }
        private void OnPointerExited(object sender, PointerEventArgs e)
        {
            Button button = (Button)sender;
            if (button != null)
            {
                button.Background = Color.FromArgb("#EFA30B");
                button.TextColor = Colors.White;
                button.FontAttributes = FontAttributes.None;
            }
        }
    }
}