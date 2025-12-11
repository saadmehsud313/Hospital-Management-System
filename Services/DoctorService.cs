using Hospital_Management_System.Repositories;
using Hospital_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management_System.Services
{
    public class DoctorService
    {
        private readonly DoctorRepository _doctorRepository;
        public DoctorService(DoctorRepository doctorRepository) 
        {
            _doctorRepository = doctorRepository;
        }
        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepository.GetAllDOctors();
        }
        public async Task<int> GetDocIDByStaffId(int staffId)
        {
            return await _doctorRepository.GetDocIDByStaffId(staffId);
        }
    }
}
