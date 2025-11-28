using Hospital_Management_System.Repository;
using Hospital_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management_System.Services
{
    public class PatientService
    {
        private readonly PatientRepository _patientRepository;
        public PatientService(PatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public void AddPatient(Patient patient)
        {     
            _patientRepository.AddPatient(patient);
        }
        
    }
}
