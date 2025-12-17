using Hospital_Management_System.Views;
using Hospital_Management_System.Models;
namespace Hospital_Management_System
{
    public partial class AppShell : Shell
    {
        public AppShell(string role)
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AppointmentView),typeof(AppointmentView));
            Routing.RegisterRoute(nameof(AdmitPatientView),typeof(AdmitPatientView));
            Routing.RegisterRoute(nameof(VisitPage),typeof(VisitPage));
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
                    Title="Manage Appointments",
                    Items =
                    {
                        new ShellContent
                        {
                            ContentTemplate=new DataTemplate(typeof(AddPatientView))
                        }
                    }
                });
                Items.Add(new FlyoutItem
                {
                    Title="Manage Admissions",
                    Items =
                    {
                        new ShellContent{
                            ContentTemplate=new DataTemplate(typeof(AdmitPatientView))
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
            else if(role.Equals("Nurse"))
            {
                Items.Clear();
                Items.Add(new FlyoutItem
                {
                    Title = "Profile",
                    Items =
                    {
                        new ShellContent{
                            ContentTemplate=new DataTemplate(typeof(ReceptionistView))
                    }
                    }
                });
                Items.Add(new FlyoutItem
                {
                    Title = "Assigned Room Details",
                    Items =
                    {
                        new ShellContent{
                            ContentTemplate=new DataTemplate(typeof(NursePage))
                    }
                    }
                });
            }
        }
    }
}
