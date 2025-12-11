using Hospital_Management_System.Views;
using Hospital_Management_System.Models;
namespace Hospital_Management_System
{
    public partial class AppShell : Shell
    {
        public AppShell(string role)
        {
            InitializeComponent();
            SetupFlyout(role);
        }
        public void SetupFlyout(string role)
        {
            Items.Clear();
            if(role.Equals("Database Admin") || role.Equals("Receptionist"))
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
                    Title="Create Appointment",
                    Items =
                    {
                        new ShellContent
                        {
                            ContentTemplate=new DataTemplate(typeof(AddPatientView))
                        }
                    }
                });
            }
            else if (role.Equals("Doctor"))
            {
                Items.Clear();
                Items.Add(new FlyoutItem
                {
                    Title = "View Appointments",
                    Items =
                    {
                        new ShellContent{
                            ContentTemplate=new DataTemplate(typeof(DoctorPage))
                    }
                    }
                });

            }
        }
    }
}
