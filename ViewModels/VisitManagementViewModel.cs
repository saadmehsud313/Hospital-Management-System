using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hospital_Management_System.Models;

namespace Hospital_Management_System.ViewModels
{
    public partial class VisitManagementViewModel : ObservableObject
    {
        // ============================================
        // DOCTOR INFORMATION (Sidebar)
        // ============================================

        [ObservableProperty]
        private string doctorName;

        [ObservableProperty]
        private string doctorId;

        [ObservableProperty]
        private string doctorSpecialization;

        [ObservableProperty]
        private string doctorDepartment;

        [ObservableProperty]
        private string doctorEmail;

        [ObservableProperty]
        private string doctorPhone;

        [ObservableProperty]
        private decimal consultationFee;

        // ============================================
        // VISIT FORM FIELDS
        // ============================================

        [ObservableProperty]
        private string patientId;

        [ObservableProperty]
        private string appointmentId;

        [ObservableProperty]
        private DateTime visitDate = DateTime.Now;

        [ObservableProperty]
        private string symptoms;

        [ObservableProperty]
        private string diagnosisSummary;

        [ObservableProperty]
        private string treatment;

        [ObservableProperty]
        private string prescription;

        [ObservableProperty]
        private DateTime followUpDate = DateTime.Today.AddDays(7);

        [ObservableProperty]
        private string selectedVisitType = "General Check Up";

        [ObservableProperty]
        private DateTime todayDate = DateTime.Today;

        // ============================================
        // ADMISSION REQUEST FIELDS
        // ============================================

        [ObservableProperty]
        private bool requestAdmission = false;

        [ObservableProperty]
        private string admissionReason;

        // ============================================
        // CONSTRUCTOR
        // ============================================

        public VisitManagementViewModel()
        {
            // Initialize with default values or load doctor data
            // LoadDoctorData();
        }

        // ============================================
        // COMMANDS (UI Interactions Only)
        // ============================================

        [RelayCommand]
        private void ClearForm()
        {
            PatientId = string.Empty;
            AppointmentId = string.Empty;
            Symptoms = string.Empty;
            DiagnosisSummary = string.Empty;
            Treatment = string.Empty;
            Prescription = string.Empty;
            FollowUpDate = DateTime.Today.AddDays(7);
            SelectedVisitType = "General Check Up";
            RequestAdmission = false;
            AdmissionReason = string.Empty;
        }

        [RelayCommand]
        private async Task SaveVisit()
        {
            // Validation
            if (string.IsNullOrWhiteSpace(PatientId))
            {
                await Shell.Current.DisplayAlertAsync(
                    "Validation Error",
                    "Please enter Patient ID",
                    "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Symptoms))
            {
                await Shell.Current.DisplayAlertAsync(
                    "Validation Error",
                    "Please enter patient symptoms",
                    "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(DiagnosisSummary))
            {
                await Shell.Current.DisplayAlertAsync(
                    "Validation Error",
                    "Please enter diagnosis summary",
                    "OK");
                return;
            }

            if (RequestAdmission && string.IsNullOrWhiteSpace(AdmissionReason))
            {
                await Shell.Current.DisplayAlertAsync(
                    "Validation Error",
                    "Please enter reason for admission",
                    "OK");
                return;
            }

            // TODO: Add your save logic here
            // await _visitService.CreateVisitAsync(visit);

            await Shell.Current.DisplayAlertAsync(
                "Success",
                "Visit recorded successfully!",
                "OK");

            ClearForm();
        }

        [RelayCommand]
        private async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }

        // ============================================
        // HELPER METHODS (For Loading Data)
        // ============================================

        public void LoadDoctorData(string doctorId)
        {
            // TODO: Load doctor data from service
            // var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);
            // DoctorName = doctor.FullName;
            // DoctorId = doctor.DoctorId;
            // etc...
        }
    }
}


// ============================================
// CODE-BEHIND (VisitManagementView.xaml.cs)
// ============================================

/*
using Hospital_Management_System.ViewModels;

namespace Hospital_Management_System.Views
{
    public partial class VisitManagementView : ContentPage
    {
        public VisitManagementView(VisitManagementViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
*/


// ============================================
// REGISTRATION (MauiProgram.cs)
// ============================================

/*
// Register in MauiProgram.cs:

builder.Services.AddTransient<VisitManagementViewModel>();
builder.Services.AddTransient<VisitManagementView>();

// Register route in AppShell.xaml.cs:
Routing.RegisterRoute(nameof(VisitManagementView), typeof(VisitManagementView));
*/