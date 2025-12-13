using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using Hospital_Management_System.Views;

namespace Hospital_Management_System.ViewModels
{

    [QueryProperty(nameof(SelectedAppointment), "SelectedAppointment")]
    public partial class AppointmentViewModel : ObservableObject
    {
        [ObservableProperty]
        Appointment selectedAppointment;

        private readonly AppointmentService _appointmentService;
        private readonly PatientService _patientService;
        private readonly DoctorService _doctorService;
        private readonly StaffService _staffService;

        // BUTTON COLOR BINDINGS

        [ObservableProperty]
        private Color updateButtonColor = Color.FromArgb("#4A90E2");

        [ObservableProperty]
        private Color cancelButtonColor = Color.FromArgb("#E74C3C");

        // STATUS BADGE COLOR BINDING

        [ObservableProperty]
        private Color statusBadgeColor = Color.FromArgb("#F39C12");

        // VIEW VISIBILITY PROPERTIES

        [ObservableProperty]
        private bool showAppointmentDetails = true;

        [ObservableProperty]
        private bool showUpdateForm = false;

        // BUTTON ENABLE/DISABLE PROPERTIES

        [ObservableProperty]
        private bool canUpdateAppointment = true;

        [ObservableProperty]
        private bool canCancelAppointment = true;

        // APPOINTMENT OBJECT

       

        // APPOINTMENT INFORMATION PROPERTIES

        [ObservableProperty]
        private string appointmentId;

        [ObservableProperty]
        private string status;

        [ObservableProperty]
        private DateTime appointmentDate;

        [ObservableProperty]
        private TimeSpan appointmentTime;

        public string AppointmentDateDisplay => AppointmentDate.ToString("dd MMMM yyyy");
        public string AppointmentTimeDisplay => DateTime.Today.Add(AppointmentTime).ToString("hh:mm tt");

        // PATIENT INFORMATION PROPERTIES
     
        [ObservableProperty]
        private string patientId;

        [ObservableProperty]
        private string patientName;

        [ObservableProperty]
        private int patientAge;

        public string PatientAgeDisplay => $"{PatientAge} years";

        [ObservableProperty]
        private string patientGender;

        [ObservableProperty]
        private string patientPhone;

        // DOCTOR INFORMATION PROPERTIES
        
        [ObservableProperty]
        private string doctorId;

        [ObservableProperty]
        private string doctorName;

        [ObservableProperty]
        private string doctorSpecialization;

        [ObservableProperty]
        private string department;

        // APPOINTMENT DETAILS PROPERTIES
        
        [ObservableProperty]
        private string reasonForVisit;

        [ObservableProperty]
        private string notes;

        // ============================================
        // BOOKING INFORMATION PROPERTIES
        // ============================================

        [ObservableProperty]
        private DateTime bookedOn;

        public string BookedOnDisplay => BookedOn.ToString("dd MMM yyyy, hh:mm tt");

        [ObservableProperty]
        private string bookedBy;

        [ObservableProperty]
        private bool hasBeenUpdated;

        [ObservableProperty]
        private DateTime lastUpdated;

        public string LastUpdatedDisplay => LastUpdated.ToString("dd MMM yyyy, hh:mm tt");

        // UPDATE FORM PROPERTIES
       
        [ObservableProperty]
        private DateTime newAppointmentDate = DateTime.Today;

        [ObservableProperty]
        private TimeSpan newAppointmentTime = new TimeSpan(10, 0, 0);

        [ObservableProperty]
        private string newReasonForVisit;

        [ObservableProperty]
        private string newNotes;

        [ObservableProperty]
        private DateTime today = DateTime.Today;

        // CONSTRUCTOR
       
        public AppointmentViewModel()
        {
            _appointmentService = MauiProgram.Services.GetRequiredService<AppointmentService>();
            _patientService = MauiProgram.Services.GetRequiredService<PatientService>();
            _doctorService = MauiProgram.Services.GetRequiredService<DoctorService>();
            _staffService = MauiProgram.Services.GetRequiredService<StaffService>();
        }

        // QUERY PROPERTY HANDLER

        partial void OnSelectedAppointmentChanged(Appointment value)
        {
            if (value != null)
            {
                LoadAppointmentDetails(value);
            }
        }

        // ============================================
        // COMMANDS
        // ============================================

        [RelayCommand]
        private void UpdateAppointment()
        {
            ShowAppointmentDetails = false;
            ShowUpdateForm = true;

            NewAppointmentDate = AppointmentDate;
            NewAppointmentTime = AppointmentTime;
            NewReasonForVisit = ReasonForVisit;
            NewNotes = Notes;
        }

        [RelayCommand]
        private async Task CancelAppointment()
        {
            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Cancel Appointment",
                $"Are you sure you want to cancel appointment {AppointmentId}?",
                "Yes, Cancel",
                "No");

            if (confirm)
            {
                try
                {
                    SelectedAppointment.Status = "Cancelled";
                    await _appointmentService.UpdateAppointmentStatusAsync(SelectedAppointment.AppointmentID,"Cancelled");

                    Status = "Cancelled";
                    StatusBadgeColor = Color.FromArgb("#95A5A6");

                    CanUpdateAppointment = false;
                    CanCancelAppointment = false;
                    UpdateButtonColor = Color.FromArgb("#BDC3C7");
                    CancelButtonColor = Color.FromArgb("#BDC3C7");

                    await Shell.Current.DisplayAlertAsync(
                        "Success",
                        "Appointment has been cancelled successfully.",
                        "OK");
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlertAsync(
                        "Error",
                        $"Failed to cancel appointment: {ex.Message}",
                        "OK");
                }
            }
        }

        [RelayCommand]
        private void CancelUpdate()
        {
            ShowUpdateForm = false;
            ShowAppointmentDetails = true;
        }

        [RelayCommand]
        private async Task SaveUpdate()
        {
            if (string.IsNullOrWhiteSpace(NewReasonForVisit))
            {
                await Shell.Current.DisplayAlertAsync(
                    "Validation Error",
                    "Please enter a reason for visit.",
                    "OK");
                return;
            }

            try
            {
                DateTime combinedDateTime = NewAppointmentDate.Date.Add(NewAppointmentTime);

                SelectedAppointment.AppointmentDate = combinedDateTime;
                SelectedAppointment.Reason = NewReasonForVisit;

                //await _appointmentService.UpdateAppointmentAsync(SelectedAppointment);

                AppointmentDate = NewAppointmentDate;
                AppointmentTime = NewAppointmentTime;
                ReasonForVisit = NewReasonForVisit;
                Notes = NewNotes;
                HasBeenUpdated = true;
                LastUpdated = DateTime.Now;

                OnPropertyChanged(nameof(AppointmentDateDisplay));
                OnPropertyChanged(nameof(AppointmentTimeDisplay));

                ShowUpdateForm = false;
                ShowAppointmentDetails = true;

                await Shell.Current.DisplayAlertAsync(
                    "Success",
                    "Appointment has been updated successfully.",
                    "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to update appointment: {ex.Message}",
                    "OK");
            }
        }

        [RelayCommand]
        private async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }

        // HELPER METHODS

        private async void LoadAppointmentDetails(Appointment appointment)
        {
            try
            {
                AppointmentId = appointment.AppointmentID.ToString();
                Status = appointment.Status;
                AppointmentDate = appointment.AppointmentDate.Date;
                AppointmentTime = appointment.AppointmentDate.TimeOfDay;
                ReasonForVisit = appointment.Reason ?? "";
                Notes = "";
                BookedOn = appointment.AppointmentDate;

                var patient = await _patientService.GetPatient(appointment.PatientID);
                if (patient != null)
                {
                    PatientId = patient.PatientId.ToString();
                    PatientName = $"{patient.FirstName} {patient.LastName}";
                    PatientAge = patient.Age;
                    PatientGender = patient.Gender;
                    PatientPhone = patient.Phone ?? "";
                }

                var doctor = await _doctorService.GetDoctorByIDAsync(appointment.DoctorID);
                var doctorStaff = await _staffService.GetStaff(doctor.StaffID);
                if (doctor != null)
                {
                    DoctorId = doctorStaff.StaffID.ToString();
                    DoctorName = doctorStaff.FullName;
                    DoctorSpecialization = doctor.Speciality;
                    Department = doctorStaff.DepartmentName;
                }

                BookedBy = $"Staff ID: {appointment.CreatedByStaff}";

                UpdateStatusBadgeColor();

                OnPropertyChanged(nameof(AppointmentDateDisplay));
                OnPropertyChanged(nameof(AppointmentTimeDisplay));
                OnPropertyChanged(nameof(PatientAgeDisplay));
                OnPropertyChanged(nameof(BookedOnDisplay));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    $"Failed to load appointment details: {ex.Message}",
                    "OK");
            }
        }

        private void UpdateStatusBadgeColor()
        {
            StatusBadgeColor = Status?.ToLower() switch
            {
                "scheduled" => Color.FromArgb("#F39C12"),
                "confirmed" => Color.FromArgb("#3498DB"),
                "completed" => Color.FromArgb("#27AE60"),
                "cancelled" => Color.FromArgb("#95A5A6"),
                "no show" => Color.FromArgb("#E74C3C"),
                "pending" => Color.FromArgb("#F39C12"),
                _ => Color.FromArgb("#7F8C8D")
            };

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            string statusLower = Status?.ToLower() ?? "";
            if (statusLower == "completed" || statusLower == "cancelled" || statusLower == "no show")
            {
                CanUpdateAppointment = false;
                CanCancelAppointment = false;
                UpdateButtonColor = Color.FromArgb("#BDC3C7");
                CancelButtonColor = Color.FromArgb("#BDC3C7");
            }
            else
            {
                CanUpdateAppointment = true;
                CanCancelAppointment = true;
                UpdateButtonColor = Color.FromArgb("#4A90E2");
                CancelButtonColor = Color.FromArgb("#E74C3C");
            }
        }
    }
}

