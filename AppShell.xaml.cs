using Hospital_Management_System.Views;
using Hospital_Management_System.Models;
namespace Hospital_Management_System
{
    public partial class AppShell : Shell
    {
        public AppShell(string role)
        {
            InitializeComponent();
            setupFlyout(role);
        }
        public void setupFlyout(string role)
        {
            Items.Clear();
            if(role.Equals("Database Admin"))
            {
                Items.Add(new FlyoutItem
                {
                    Title="Profile",
                    Items ={
                        new ShellContent
                    {
                        ContentTemplate=new DataTemplate(typeof(ReceptionistView))
                    }
                    }
                    
                });
                Items.Add(new FlyoutItem
                {
                    Title="Add Patients",
                    Items =
                    {
                        new ShellContent
                        {
                            ContentTemplate=new DataTemplate(typeof(AddPatientView))
                        }
                    }
                });
            }
        }
    }
}
