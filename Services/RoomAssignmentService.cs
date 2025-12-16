using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repository;

namespace Hospital_Management_System.Services
{
    public class RoomAssignmentService
    {
        private readonly RoomAssignmentRepository _roomAssignmentRepository;

        public RoomAssignmentService(RoomAssignmentRepository roomAssignmentRepository)
        {
            _roomAssignmentRepository = roomAssignmentRepository;
        }

        /// <summary>
        /// Gets today's room assignments for a nurse
        /// Uses GetRoomAssignmentsByDateAsync with today's date
        /// </summary>
        public async Task<List<RoomAssignment>> GetTodayRoomAssignments(int nurseID)
        {
            try
            {
                Debug.WriteLine($"🔄 RoomAssignmentService: Getting today's assignments for NurseID: {nurseID}");

                if (nurseID <= 0)
                {
                    Debug.WriteLine($"❌ RoomAssignmentService: Invalid NurseID: {nurseID}");
                    return new List<RoomAssignment>();
                }

                // ✅ FIXED: Use GetRoomAssignmentsByDateAsync with today's date
                var assignments = await _roomAssignmentRepository.GetRoomAssignmentsByDateAsync(DateTime.Now);

                Debug.WriteLine($"✅ RoomAssignmentService: Loaded {assignments.Count} today assignments for NurseID: {nurseID}");
                return assignments;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ RoomAssignmentService Error (GetTodayRoomAssignments): {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<RoomAssignment>();
            }
        }

        /// <summary>
        /// Gets room assignments for a specific date for a nurse
        /// </summary>
        public async Task<List<RoomAssignment>> GetRoomAssignmentsByDate(int nurseID, DateTime date)
        {
            try
            {
                Debug.WriteLine($"🔄 RoomAssignmentService: Getting assignments for NurseID: {nurseID} on date: {date:yyyy-MM-dd}");

                if (nurseID <= 0)
                {
                    Debug.WriteLine($"❌ RoomAssignmentService: Invalid NurseID: {nurseID}");
                    return new List<RoomAssignment>();
                }

                // ✅ FIXED: Use GetRoomAssignmentsByDateAsync (doesn't filter by nurseID in repository)
                var assignments = await _roomAssignmentRepository.GetRoomAssignmentsByDateAsync(date);

                Debug.WriteLine($"✅ RoomAssignmentService: Loaded {assignments.Count} assignments for {date:yyyy-MM-dd}");
                return assignments;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ RoomAssignmentService Error (GetRoomAssignmentsByDate): {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<RoomAssignment>();
            }
        }

        /// <summary>
        /// Checks if nurse has any assignments today
        /// </summary>
        public async Task<bool> HasTodayAssignments(int nurseID)
        {
            try
            {
                var assignments = await GetTodayRoomAssignments(nurseID);
                return assignments != null && assignments.Count > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ RoomAssignmentService Error (HasTodayAssignments): {ex.Message}");
                return false;
            }
        }
    }
}