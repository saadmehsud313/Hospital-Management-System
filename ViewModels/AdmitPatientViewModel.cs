using CommunityToolkit.Mvvm.ComponentModel;
using Hospital_Management_System.Services;

namespace Hospital_Management_System.ViewModels
{
   partial class AdmitPatientViewModel : ObservableObject
    {
        private readonly PatientService _patientService;
        AdmitPatientViewModel(PatientService patientService)
        {
            _patientService = patientService;
        }

    }
}
