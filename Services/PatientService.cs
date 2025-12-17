using Hospital_Management_System.Repository;
using Hospital_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Hospital_Management_System.Services
{
    public class PatientService
    {
        private readonly PatientRepository _patientRepository;
        public PatientService(PatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        
        public async Task<bool> AddPatient(Patient patient)
        {     
            patient.PatientId = await _patientRepository.GetLastPatientAsync()+1;
            return await _patientRepository.AddPatientAsync(patient);
        }
        public async Task<Patient> GetPatient(int patientId)
        {
            return await _patientRepository.GetPatientAsync(patientId);
        }
        
    }
}
