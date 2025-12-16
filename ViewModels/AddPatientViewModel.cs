using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Runtime;
using Hospital_Management_System.Models;
using Hospital_Management_System.Views;
using Hospital_Management_System.Services;
using System.Diagnostics;
using Hospital_Management_System.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hospital_Management_System.Repository;

namespace Hospital_Management_System.ViewModels
{
    public partial class AddPatientViewModel : ObservableObject
    {
        private readonly PatientService _patientService;
        private readonly DoctorService _doctorService;
        private readonly AppointmentService _appointmentService;
        private Patient _patient;

        [ObservableProperty]
        string firstName;
        [ObservableProperty]
        string lastName;
        [ObservableProperty]
        DateTime? appointmentDate;
        [ObservableProperty]
        DateOnly today;
        [ObservableProperty]
        String selectedBloodGroup;
        [ObservableProperty]
        string selectedGender;
        [ObservableProperty]
        string phone;
        [ObservableProperty]
        string reason;
        [ObservableProperty]
        string emergencyContactName;
        [ObservableProperty]
        string emergencyContactPhone;
        [ObservableProperty]
        string doctorName;
        [ObservableProperty]
        Doctor doctorSelected;
        [ObservableProperty]
        string selectedSpecialization;
        [ObservableProperty]
        string staffName;
        [ObservableProperty]
        string age;
        [ObservableProperty]
        string departmentName;
        [ObservableProperty]
        string role;
        [ObservableProperty]
        List<string> specializations;
        [ObservableProperty]
        ObservableCollection<Doctor> filteredDoctors;
        List<Doctor> allDoctors;
        [ObservableProperty]
        ObservableCollection<Appointment> appointments;
        [ObservableProperty]
        bool toggleNewAppointment;
        [ObservableProperty]
        bool toggleManageAppointment;
        [ObservableProperty]
        Appointment selectedAppointment;
        [ObservableProperty]
        TimeSpan appointmentTime;
        [ObservableProperty]
        bool toggleAllAppointment = false;

        //Constructor to initialize services and load data
        public AddPatientViewModel(PatientService patientService, DoctorService doctorService, AppointmentService appointmentService)
        {
            Today = DateOnly.FromDateTime(DateTime.Now);
            _patientService = patientService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            ToggleNewAppointment = true;
            ToggleManageAppointment = false;
            LoadStaffData();
            allDoctors = _doctorService.GetAllDoctors();
            specializations = allDoctors.Select(d => d.Speciality).Distinct().ToList();
            filteredDoctors = new ObservableCollection<Doctor>();

        }
        //Update the doctor picker an only shows name of doctors with the selected specialization
        partial void OnSelectedSpecializationChanged(string value)
        {
            FilteredDoctors.Clear();
            if (!string.IsNullOrEmpty(value))
            {
                var doctors = allDoctors.Where(d => d.Speciality == Specializations[Convert.ToInt32(value)]);
                foreach (Doctor doc in doctors)
                {
                    FilteredDoctors.Add(doc);
                }
            }
        }


        private bool HasSpecialChars(string yourString)
        {
            return yourString.Any(ch => !char.IsLetterOrDigit(ch));
        }
        //Verify all required fields are filled
        private bool VerifyPatient()
        {
            if (FirstName.IsWhiteSpace() || LastName is null)
            {
                
                Shell.Current.DisplayAlertAsync("Error", "First Name and Last Name are required.", "OK");
                return false;
            }
            else if ((HasSpecialChars(FirstName) && FirstName.Length > 10) || (HasSpecialChars(LastName) && LastName.Length>10))
            {
                Shell.Current.DisplayAlertAsync("Error", "First Name or Last Name contains special characters or is too long.", "OK");
                return false;

            }
            else if (AppointmentDate.Equals(null))
            {
                Shell.Current.DisplayAlertAsync("Error", "Appointment Date is required.", "OK");
            }
            else if (SelectedBloodGroup is null)
            {
                Shell.Current.DisplayAlertAsync("Error", "Please select a Blood Group.", "OK");
                return false;

            }
            else if (String.IsNullOrEmpty(Age))
            {
                Shell.Current.DisplayAlertAsync("Error", "Enter a valid age.", "OK");
                return false;
            }
            else if (!(int.TryParse(Age, out int ageValue)))
            {
                Shell.Current.DisplayAlertAsync("Error", "Please enter a valid Age.", "OK");
                return false;
            }
            else if (ageValue <= 0 || ageValue > 120)
            {
                Shell.Current.DisplayAlertAsync("Error", "Please enter age between 0 and 120.", "OK");
                return false;
            }
            else if (DoctorSelected is null)
            {
                Shell.Current.DisplayAlertAsync("Error", "Please select a Doctor.", "OK");
                return false;
            }
            else if (SelectedGender is null)
            {
                Shell.Current.DisplayAlertAsync("Error", "Please select a Gender.", "OK");
            }
            else if (String.IsNullOrEmpty(Phone) || Phone.Length != 11 || !Phone.All(char.IsDigit))
            {
                Shell.Current.DisplayAlertAsync("Error", "Please enter a valid 11-digit Phone Number.", "OK");
                return false;
            }
            else if(String.IsNullOrEmpty(EmergencyContactName) || HasSpecialChars(EmergencyContactName) || EmergencyContactName.Length>20)
            {
                Shell.Current.DisplayAlertAsync("Error", "Emergency Contact Name is required.", "OK");
                return false;
            }
            else if (!String.IsNullOrEmpty(EmergencyContactPhone) && (EmergencyContactPhone.Length != 11 || !EmergencyContactPhone.All(char.IsDigit)))
            {
                Shell.Current.DisplayAlertAsync("Error", "Please enter a valid 11-digit Emergency Contact Phone Number.", "OK");
                return false;
            }
            else
            {
                return true;
            }
            return false;
        }



        //Load the staff data for the logged in user
        public void LoadStaffData()
        {
            StaffName = ReceptionistViewModel.user.Username;
            Role = ReceptionistViewModel.user.Role;
            DepartmentName = ReceptionistViewModel.user.DepartmentName;
        }


        /*Function to add patient to the database binded to
        the Add Patient button*/
        [RelayCommand]
        public async Task AddPatient()
        {
            //Merges The Date And time from the separate pickers into one Datetime object
            AppointmentDate = AppointmentDate + AppointmentTime;
            //Verifies that no input is left empty 
            if (VerifyPatient())
            {
                _patient = new Patient
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    CreatedAt = DateTime.Now,
                    BloodGroup = SelectedBloodGroup,
                    Gender = SelectedGender,
                    Phone = Phone,
                    Age = Convert.ToInt32(Age),
                    EmergencyContactName = EmergencyContactName,
                    EmergencyContactPhone = EmergencyContactPhone
                };

                await _patientService.AddPatient(_patient);
                Appointment appointment = new()
                {
                    DoctorID = DoctorSelected.DoctorId,
                    CreatedByStaff = ReceptionistViewModel.user.DocOrStaffId,
                    PatientID = _patient.PatientId,
                    AppointmentDate = AppointmentDate ?? DateTime.Now,
                    Status = "Scheduled",
                    Reason = Reason
                };
                if (await _appointmentService.ScheduleAppointment(appointment))
                {
                    await Shell.Current.DisplayAlertAsync("Success", "Patient and Appointment added successfully.", "OK");
                    FirstName = string.Empty;
                    LastName = string.Empty;
                    AppointmentDate = null;
                    AppointmentTime = TimeSpan.Zero;
                    SelectedBloodGroup = null;
                    SelectedGender = null;
                    Phone = string.Empty;
                    EmergencyContactName = string.Empty;
                    EmergencyContactPhone = string.Empty;
                    Reason = null;
                    SelectedSpecialization = null;
                    Age = string.Empty;
                    DoctorSelected = null;

                }
                else
                {
                    await Shell.Current.DisplayAlertAsync("Error", "Failed to add Appointment.", "OK");
                }
            }
        }
        [RelayCommand]
        public async Task LoadScheduledAppointments()
        {
            Appointments = new ObservableCollection<Appointment>(await _appointmentService.GetScheduledAppointmentsAsync());
        }

        [RelayCommand]
        public void ShowNewAppointment()
        {
            ToggleManageAppointment = false;
            ToggleNewAppointment = true;
            ToggleAllAppointment = false;
        }
        [RelayCommand]
        public async Task ShowManageAppointment()
        {
            ToggleNewAppointment = false;
            ToggleManageAppointment = true;
            ToggleAllAppointment = false;
            await LoadScheduledAppointments();
        }
        [RelayCommand]
        public async Task OnTapped(Appointment appointment)
        {
            await Shell.Current.GoToAsync(nameof(AppointmentView), true, new Dictionary<string, object>
            {
                { "SelectedAppointment",appointment}
            });
        }
        [RelayCommand]
        public  void ShowAllAppointment()
        {
            ToggleNewAppointment = false;
            ToggleManageAppointment = false;
            ToggleAllAppointment = true;
            LoadAllAppointments();
        }
        public async Task OnAppearing()
        {
            await LoadScheduledAppointments();
        }
        private async void LoadAllAppointments()
        {
            var _appointmentRepository=MauiProgram.Services.GetRequiredService<AppointmentRepository>();
            Appointments =new ObservableCollection<Appointment>(await _appointmentRepository.GetAllAppointmentsAsync());
        }
        //[ObservableProperty]
        //Color statusBadgeColor;
        //private void UpdateStatusBadgeColor()
        //{
        //    StatusBadgeColor = Status?.ToLower() switch
        //    {
        //        "scheduled" => Color.FromArgb("#F39C12"),
        //        "confirmed" => Color.FromArgb("#3498DB"),
        //        "completed" => Color.FromArgb("#27AE60"),
        //        "cancelled" => Color.FromArgb("#95A5A6"),
        //        "no show" => Color.FromArgb("#E74C3C"),
        //        "pending" => Color.FromArgb("#F39C12"),
        //        _ => Color.FromArgb("#7F8C8D")
        //    };
        //}
        }
}
