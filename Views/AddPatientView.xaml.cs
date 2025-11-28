
namespace Hospital_Management_System.Views;
    public partial class AddPatientView : ContentPage
    {
        public AddPatientView()
        {
            InitializeComponent();
        }
        private void OnPointerEntered(object sender, PointerEventArgs e)
        {
            Button button = (Button)sender;
            if (button != null)
            {
                button.TextColor = Color.FromArgb("#EC9C13");
                button.BorderWidth = 1;
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
