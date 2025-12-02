using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Runtime;
using Hospital_Management_System.Models;
using Hospital_Management_System.Views;
using Hospital_Management_System.Services;
using System.Diagnostics;
using Hospital_Management_System.Repositories;
namespace Hospital_Management_System.ViewModels
{
    public partial class AddPatientViewModel : ObservableObject
    {
        private readonly PatientService _patientService;
        private readonly DoctorService _doctorService;
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
        string selectedBloodGroup;
        [ObservableProperty]
        string selectedGender;
        [ObservableProperty]
        string phone;
        [ObservableProperty]
        string emergencyContactName;
        [ObservableProperty]
        string emergencyContactPhone;
        [ObservableProperty]
        List<Doctor> doctors;
        [ObservableProperty]
        string doctorName;
        [ObservableProperty]
        string doctorSelected;
        //[ObservableProperty]
        //List<Staff> doctors;
        [ObservableProperty]
        string staffName;
        [ObservableProperty]
        string departmentName;
        [ObservableProperty]
        string role;
        public AddPatientViewModel(PatientService patientService, DoctorService doctorService)
        {
            Today = DateOnly.FromDateTime(DateTime.Now);
            _patientService = patientService;
            _doctorService = doctorService;
            LoadStaffData();
            doctors = _doctorService.GetAllDoctors();
        }
        private bool VerifyPatient()
        {
            if (FirstName.IsWhiteSpace() || LastName is null)
            {
                Shell.Current.DisplayAlertAsync("Error", "First Name and Last Name are required.", "OK");
                return false;
            }
            else if (AppointmentDate.Equals(null))
            {
                Shell.Current.DisplayAlertAsync("Error", "Appointment Date is required.", "OK");
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
            else
            {
                return true;
            }
            return false;
        }

        public void LoadStaffData()
        {
            StaffName = ReceptionistViewModel.user.Username;
            Role = ReceptionistViewModel.user.Role;
            DepartmentName = ReceptionistViewModel.user.DepartmentName;
        }

        [RelayCommand]
        public async Task AddPatient()
        {
            if (true)
            {
                _patient = new Patient
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    CreatedAt = AppointmentDate,
                    BloodGroup = SelectedBloodGroup,
                    Gender = SelectedGender,
                    Phone = Phone,
                    EmergencyContactName = EmergencyContactName,
                    EmergencyContactPhone = EmergencyContactPhone
                };
                await _patientService.AddPatient(_patient);
                

            }
        }
    }
}
