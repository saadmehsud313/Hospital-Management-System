using Hospital_Management_System.Repositories;
using Hospital_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management_System.Services
{
    public class StaffService
    {
        private readonly StaffRepository _staffRepository;
        public StaffService(StaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }
        public async Task<Staff> GetStaff(int id)
        {
            Staff staff= await _staffRepository.GetStaffData(id);
            if (staff is not null)
            { return staff; }
            else
            { return null; }
        }
    }
}
