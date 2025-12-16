using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Hospital_Management_System.Repository;

namespace Hospital_Management_System.Services
{
    public class NurseService
    {
        private readonly NurseRepository _nurseRepository;

        public NurseService(NurseRepository nurseRepository)
        {
            _nurseRepository = nurseRepository;
        }

        public async Task<int> GetNurseIDByStaffId(int staffId)
        {
            try
            {
                Debug.WriteLine($"NurseService: Getting NurseID for StaffID: {staffId}");

                int nurseId = await _nurseRepository.GetNurseIDByStaffIdAsync(staffId);

                if (nurseId == -1)
                {
                    Debug.WriteLine($"NurseService: No NurseID found for StaffID: {staffId}");
                    return -1;
                }

                Debug.WriteLine($"NurseService: Successfully retrieved NurseID: {nurseId} for StaffID: {staffId}");
                return nurseId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"NurseService Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return -1;
            }
        }
    }
}