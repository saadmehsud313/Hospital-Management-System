using Hospital_Management_System.Repositories;
using Hospital_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
            { 
                return staff; }
            else
            { 
                return null; }
        }
        //public async Task<bool> UpdateUsername(string staffId, string newUsername)
        //{
        //    try
        //    {
        //        Debug.WriteLine($"🔄 StaffService: Updating username for staff {staffId}");
        //        return await _staffRepository.UpdateUsernameAsync(staffId, newUsername);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"❌ StaffService Error in UpdateUsername: {ex.Message}");
        //        return false;
        //    }
        //}

        // Update password only
        public async Task<bool> UpdatePassword(string staffId, string newPasswordHash)
        {
            try
            {
                Debug.WriteLine($"🔄 StaffService: Updating password for staff {staffId}");
                return await _staffRepository.UpdatePasswordAsync(staffId, newPasswordHash);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ StaffService Error in UpdatePassword: {ex.Message}");
                return false;
            }
        }

    }
}
