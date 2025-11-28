using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
namespace Hospital_Management_System.ViewModels
{
    partial class AddPatientViewModel : ObservableObject
    {
        private readonly PatientService _patientService;
        private Patient _patient;
        [ObservableProperty]
        string firstName;
        [ObservableProperty]
        string lastName;
        [ObservableProperty]
        DateTime appointmentDate;
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
        public AddPatientViewModel(PatientService patientService)
        {
            Today = DateOnly.FromDateTime(DateTime.Now);
            _patientService = patientService;
        }

        [RelayCommand]
        public void AddPatient()
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
            _patientService.AddPatient(_patient);
        }    
    }
}
