using Hospital_Management_System.ViewModels;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace Hospital_Management_System.Views
{
    public partial class NursePage : ContentPage
    {
        public NursePage(NurseViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        // Hover effect for buttons - makes them stand out
        private void OnPointerEntered(object sender, PointerEventArgs e)
        {
            Button button = (Button)sender;
            if (button != null)
            {
                // Check if it's a sidebar button (Profile, Room Assignments, Update Credentials)
                if (button.Text == "Profile" || button.Text == "Room Assignments" || button.Text == "Update Credentials")
                {
                    // Sidebar button hover: Dark background ? Blue background
                    button.BackgroundColor = Color.FromArgb("#4A90E2");
                    button.TextColor = Colors.White;
                    button.FontAttributes = FontAttributes.Bold;
                    button.BorderWidth = 0;
                }
                else if (button.Text == "Logout")
                {
                    // Logout button hover: Red ? Blue
                    button.BackgroundColor = Color.FromArgb("#4A90E2");
                    button.TextColor = Colors.White;
                    button.FontAttributes = FontAttributes.Bold;
                    button.BorderWidth = 0;
                }
                else if (button.Text == "TODAY" || button.Text == "HISTORY")
                {
                    // Tab buttons: Change to white text with blue background
                    button.TextColor = Colors.White;
                    button.FontAttributes = FontAttributes.Bold;
                }
                else if (button.Text == "Load History")
                {
                    // Load History button: Enhance blue
                    button.BackgroundColor = Color.FromArgb("#357ABD");
                    button.TextColor = Colors.White;
                    button.FontAttributes = FontAttributes.Bold;
                }
                else if (button.Text == "Save Changes")
                {
                    // Save Changes button: Enhance green
                    button.BackgroundColor = Color.FromArgb("#229954");
                    button.TextColor = Colors.White;
                    button.FontAttributes = FontAttributes.Bold;
                }
                else if (button.Text == "Cancel")
                {
                    // Cancel button: Border ? Blue background
                    button.BackgroundColor = Color.FromArgb("#4A90E2");
                    button.TextColor = Colors.White;
                    button.FontAttributes = FontAttributes.Bold;
                    button.BorderWidth = 0;
                }
            }
        }

        private void OnPointerExited(object sender, PointerEventArgs e)
        {
            Button button = (Button)sender;
            if (button != null)
            {
                // Return to default state based on button type
                if (button.Text == "Profile" || button.Text == "Room Assignments" || button.Text == "Update Credentials")
                {
                    // Sidebar buttons return to dark gray
                    button.BackgroundColor = Color.FromArgb("#34495E");
                    button.TextColor = Colors.White;
                    button.FontAttributes = FontAttributes.None;
                }
                else if (button.Text == "Logout")
                {
                    // Logout button returns to red
                    button.BackgroundColor = Color.FromArgb("#E74C3C");
                    button.TextColor = Colors.White;
                    button.FontAttributes = FontAttributes.None;
                }
                else if (button.Text == "TODAY" || button.Text == "HISTORY")
                {
                    // Tab buttons - keep their original colors (don't reset here, they're managed by ViewModel)
                    button.FontAttributes = FontAttributes.None;
                }
                else if (button.Text == "Load History")
                {
                    // Load History button returns to blue
                    button.BackgroundColor = Color.FromArgb("#4A90E2");
                    button.TextColor = Colors.White;
                    button.FontAttributes = FontAttributes.None;
                }
                else if (button.Text == "Save Changes")
                {
                    // Save Changes button returns to green
                    button.BackgroundColor = Color.FromArgb("#27AE60");
                    button.TextColor = Colors.White;
                    button.FontAttributes = FontAttributes.None;
                }
                else if (button.Text == "Cancel")
                {
                    // Cancel button returns to white with border
                    button.BackgroundColor = Colors.White;
                    button.TextColor = Color.FromArgb("#7F8C8D");
                    button.FontAttributes = FontAttributes.None;
                    button.BorderWidth = 2;
                    button.BorderColor = Color.FromArgb("#E1E8ED");
                }
            }
            else
            {
                Debug.WriteLine("Button is null");
            }
        }
    }
}