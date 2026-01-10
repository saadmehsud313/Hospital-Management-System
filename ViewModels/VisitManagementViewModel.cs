using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repositories;
using Hospital_Management_System.Services;
using System.Diagnostics;

namespace Hospital_Management_System.ViewModels
{
    [QueryProperty(nameof(PresentAppointment),"SelectedAppointment")]
    [QueryProperty(nameof(PresentStaff),"Doctor")]
    public partial class VisitManagementViewModel : ObservableObject
    {
        // ============================================
        // DOCTOR INFORMATION (Sidebar)
        // ============================================
        private readonly VisitService _visitService;
        [ObservableProperty]
        Appointment presentAppointment;
        [ObservableProperty]
        Staff presentStaff;

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
        public bool requestAdmission = false;

        [ObservableProperty]
        private string admissionReason;

        // ============================================
        // CONSTRUCTOR
        // ============================================

        public VisitManagementViewModel()
        {
            _visitService = MauiProgram.Services.GetRequiredService<VisitService>();

        }

        // COMMANDS (UI Interactions Only)

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
            if (string.IsNullOrEmpty(SelectedVisitType))
            {
                await Shell.Current.DisplayAlertAsync(
                    "Validation Error",
                    "Please select visit type",
                    "OK");
                return;
            }
            // TODO: Add your save logic here
            
            Visit visit = new Visit
            {
                DoctorId = int.Parse(DoctorId),
                PatientId = int.Parse(PatientId),
                AppointmentId = int.Parse(AppointmentId),
                Symptoms = Symptoms,
                DiagnosisSummary = DiagnosisSummary,
                Prescriptions = Prescription,
                VisitType = SelectedVisitType,
                VisitDateTime = VisitDate,
                CreatedAt = DateTime.Now,
                FollowUpDate = RequestAdmission ? (DateTime?)FollowUpDate : null
            };
            bool visitStatus= await _visitService.CreateVisit(visit);
            bool request = true;
            if (RequestAdmission)
            {
                var doc_service = MauiProgram.Services.GetRequiredService<DoctorService>();
                int id = await doc_service.GetDocIDByStaffId(int.Parse(DoctorId));
                Debug.WriteLine("ID:", id);
                // Create an admission request object
                RoomRequest admissionRequest = new RoomRequest
                {
                    PatientId = int.Parse(PatientId),
                    DoctorId = id,
                    Status = "Pending"
                };
                var admissionRepository = MauiProgram.Services.GetRequiredService<AdmissionRepository>();
                request = await admissionRepository.CreateAdmitRequest(admissionRequest);
            }
            if (request && visitStatus)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Success",
                    "Visit and Admission Request recorded successfully!",
                    "OK");
                _ = Back();
            }
            else if (request == false)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    "Failed to create admission request.",
                    "OK");
            }
            else if(visitStatus == false)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    "Failed to record visit.",
                    "OK");
            }
        }

        [RelayCommand]
        private async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }

        // ============================================
        // HELPER METHODS (For Loading Data)
        // ============================================

        public void LoadDoctorData(Staff staff)
        {
            if (staff == null)
            {
                return;
            }
            else
            {
                DoctorName = $"{staff.FirstName} {staff.LastName}";
                DoctorId = staff.StaffID.ToString();
                DoctorSpecialization = staff.Role;
                DoctorDepartment = staff.DepartmentName;
                DoctorEmail = staff.Email;
                DoctorPhone = staff.Phone;

            }
        }
        partial void OnPresentAppointmentChanged(Appointment value)
        {
            if (value != null)
            {
                LoadAppointmentData(value);
            }
        }
        partial void OnPresentStaffChanged(Staff value)
        {
            LoadDoctorData(value);
        }
        public void LoadAppointmentData(Appointment presentAppointment)
        {
            if (presentAppointment == null)
            {
                Debug.WriteLine("No appointment data provided.");
                return;
            }
            PatientId = presentAppointment.PatientID.ToString();
            AppointmentId = presentAppointment.AppointmentID.ToString();
        }
        //public async Task OnAppearing()
        //{

        //}
    }
}