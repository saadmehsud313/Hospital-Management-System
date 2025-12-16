using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Hospital_Management_System.Models;
using Microsoft.Data.SqlClient;

namespace Hospital_Management_System.Repository
{
    public class RoomAssignmentRepository
    {
        private readonly string _connectionString;

        public RoomAssignmentRepository(DatabaseConfig dbConfig)
        {
            _connectionString = dbConfig.ConnectionString;
        }

        /// <summary>
        /// SMART ALGORITHM: Assign room to nurse with 3-level validation
        /// 
        /// REQUIREMENT 1: A room cannot be assigned to more than one nurse in a day
        /// REQUIREMENT 2: A nurse cannot be assigned more than 10 rooms a day
        /// REQUIREMENT 3: Only the receptionist who assigned it can remove it
        /// 
        /// Returns: (Success, Message)
        /// </summary>
        public async Task<(bool Success, string Message)> AssignRoomToNurseAsync(
            int nurseId,
            int roomId,
            DateTime assignmentDate,
            int assignedByStaffId)
        {
            try
            {
                using SqlConnection connection = new(_connectionString);
                await connection.OpenAsync();

                // ========================================
                // VALIDATION 1: Room not assigned to ANY nurse on this date
                // (A room cannot be assigned to more than one nurse in a day)
                // ========================================
                string roomCheckQuery = @"
                    SELECT COUNT(*) FROM RoomAssignment 
                    WHERE RoomID = @RoomID 
                    AND CAST(AssignmentDate AS DATE) = @AssignmentDate 
                    AND IsActive = 1";

                using (SqlCommand roomCheckCmd = new(roomCheckQuery, connection))
                {
                    roomCheckCmd.Parameters.AddWithValue("@RoomID", roomId);
                    roomCheckCmd.Parameters.AddWithValue("@AssignmentDate", assignmentDate.Date);

                    int roomAssignedCount = (int)await roomCheckCmd.ExecuteScalarAsync();
                    if (roomAssignedCount > 0)
                    {
                        Debug.WriteLine($"❌ Room {roomId} already assigned on {assignmentDate:yyyy-MM-dd}");
                        return (false, $"❌ This room is already assigned to another nurse on {assignmentDate:yyyy-MM-dd}. One room can only be assigned to one nurse per day.");
                    }
                }

                // ========================================
                // VALIDATION 2: Nurse doesn't have 10+ rooms on this date
                // (A nurse cannot be assigned more than 10 rooms a day)
                // ========================================
                string countQuery = @"
                    SELECT COUNT(*) FROM RoomAssignment 
                    WHERE NurseID = @NurseID 
                    AND CAST(AssignmentDate AS DATE) = @AssignmentDate 
                    AND IsActive = 1";

                int assignmentCount = 0;
                using (SqlCommand countCmd = new(countQuery, connection))
                {
                    countCmd.Parameters.AddWithValue("@NurseID", nurseId);
                    countCmd.Parameters.AddWithValue("@AssignmentDate", assignmentDate.Date);

                    assignmentCount = (int)await countCmd.ExecuteScalarAsync();
                    if (assignmentCount >= 10)
                    {
                        Debug.WriteLine($"❌ Nurse {nurseId} already has {assignmentCount} rooms on {assignmentDate:yyyy-MM-dd}");
                        return (false, $"❌ Nurse cannot have more than 10 rooms per day. Current assignments: {assignmentCount}/10");
                    }
                }

                // ========================================
                // VALIDATION 3: Insert assignment with receptionist tracking
                // (Only creator can remove it - tracked via AssignedByStaffID)
                // ========================================
                string insertQuery = @"
                    INSERT INTO RoomAssignment 
                    (NurseID, RoomID, AssignmentDate, AssignedByStaffID, CreatedAt, IsActive)
                    VALUES 
                    (@NurseID, @RoomID, @AssignmentDate, @AssignedByStaffID, GETDATE(), 1)";

                using (SqlCommand insertCmd = new(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@NurseID", nurseId);
                    insertCmd.Parameters.AddWithValue("@RoomID", roomId);
                    insertCmd.Parameters.AddWithValue("@AssignmentDate", assignmentDate.Date);
                    insertCmd.Parameters.AddWithValue("@AssignedByStaffID", assignedByStaffId);

                    int rowsAffected = await insertCmd.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        int newCount = assignmentCount + 1;
                        Debug.WriteLine($"✅ Room {roomId} assigned to Nurse {nurseId} by Staff {assignedByStaffId} ({newCount}/10)");
                        return (true, $"✅ Room assignment successful! Nurse now has {newCount}/10 rooms assigned.");
                    }
                }

                return (false, "❌ Assignment failed. Please try again.");
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 2627)
            {
                // UNIQUE constraint violation - room already assigned by another receptionist
                Debug.WriteLine($"❌ UNIQUE constraint: Room already assigned on this date");
                return (false, "❌ This room is already assigned on this date by another receptionist!");
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine($"❌ SQL Error: {sqlEx.Message}");
                return (false, $"❌ Database error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ AssignRoomToNurseAsync error: {ex.Message}");
                return (false, $"❌ Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all unassigned (available) rooms for a specific date
        /// ALGORITHM: Find rooms NOT assigned on that date
        /// </summary>
        public async Task<List<Room>> GetAvailableRoomsAsync(DateTime assignmentDate)
        {
            List<Room> rooms = new();

            try
            {
                using SqlConnection connection = new(_connectionString);

                string query = @"
                    SELECT DISTINCT r.RoomID, r.RoomNumber, r.Ward, r.RoomType, r.Capacity, r.DailyRate
                    FROM Room r
                    WHERE r.RoomID NOT IN (
                        SELECT RoomID FROM Room_Assignment 
                        WHERE CAST(AssignmentDate AS DATE) = @AssignmentDate 
                        AND IsActive = 1
                    )
                    ORDER BY r.RoomNumber";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AssignmentDate", assignmentDate.Date);

                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    rooms.Add(new Room
                    {
                        RoomID = reader.GetInt32(0),
                        RoomNumber = reader.GetString(1),
                        Ward = reader.GetString(2),
                        RoomType = reader.GetString(3),
                        Capacity = reader.GetInt32(4),
                        DailyRate = reader.GetDouble(5)
                    });
                }

                Debug.WriteLine($"✅ Found {rooms.Count} available rooms for {assignmentDate:yyyy-MM-dd}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ GetAvailableRoomsAsync error: {ex.Message}");
            }

            return rooms;
        }

        /// <summary>
        /// Gets all active nurses
        /// </summary>
        public async Task<List<Nurse>> GetAllNursesAsync()
        {
            List<Nurse> nurses = new();

            try
            {
                using SqlConnection connection = new(_connectionString);

                string query = @"select * from Nurse_View";

                using SqlCommand command = new(query, connection);
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    nurses.Add(new Nurse
                    {
                        NurseID = reader.GetInt32(0),
                        StaffID = reader.GetInt32(1),
                        FirstName=reader.GetString(2),
                        LastName=reader.GetString(3),
                        AssignedRoomID = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                        Shift = reader.IsDBNull(5) ? "" : reader.GetString(5)
                    });
                }

                Debug.WriteLine($"✅ Loaded {nurses.Count} active nurses");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ GetAllNursesAsync error: {ex.Message}");
            }

            return nurses;
        }

        /// <summary>
        /// Gets room assignments for a specific date with patient information
        /// ALGORITHM: JOIN with Admission to show current patient in room
        /// </summary>
        public async Task<List<RoomAssignment>> GetRoomAssignmentsByDateAsync(DateTime assignmentDate)
        {
            List<RoomAssignment> assignments = new();

            try
            {
                using SqlConnection connection = new(_connectionString);

                string query = @"
                    SELECT 
                        ra.AssignmentID,
                        ra.NurseID,
                        ra.RoomID,
                        r.RoomNumber,
                        r.Ward,
                        r.RoomType,
                        ra.AssignmentDate,
                        ISNULL(p.FirstName + ' ' + p.LastName, 'No Patient') AS PatientName,
                        ISNULL(adm.AdmitReason, 'N/A') AS AdmitReason,
                        ISNULL(adm.PatientID, 0) AS PatientID,
                        ISNULL(adm.AdmissionID, 0) AS AdmissionID,
                        ra.AssignedByStaffID
                    FROM Room_Assignment ra
                    INNER JOIN Nurse n ON ra.NurseID = n.NurseID
                    INNER JOIN Room r ON ra.RoomID = r.RoomID
                    LEFT JOIN Admission adm ON ra.RoomID = adm.RoomID AND adm.Status = 'Active'
                    LEFT JOIN Patient p ON adm.PatientID = p.PatientID
                    WHERE CAST(ra.AssignmentDate AS DATE) = @AssignmentDate 
                    AND ra.IsActive = 1
                    ORDER BY r.RoomNumber";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AssignmentDate", assignmentDate.Date);

                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    assignments.Add(new RoomAssignment
                    {
                        AssignmentID = reader.GetInt32(0),
                        NurseID = reader.GetInt32(1),
                        RoomID = reader.GetInt32(2),
                        RoomNumber = reader.GetString(3),
                        Ward = reader.GetString(4),
                        RoomType = reader.GetString(5),
                        AssignmentDate = reader.GetDateTime(6),
                        PatientName = reader.GetString(7),
                        AdmitReason = reader.GetString(8),
                        PatientID = reader.GetInt32(9),
                        AdmissionID = reader.GetInt32(10),
                        AssignedByStaffID = reader.GetInt32(11)
                    });
                }

                Debug.WriteLine($"✅ Loaded {assignments.Count} assignments for {assignmentDate:yyyy-MM-dd}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ GetRoomAssignmentsByDateAsync error: {ex.Message}");
            }

            return assignments;
        }

        /// <summary>
        /// Removes an assignment (soft delete)
        /// REQUIREMENT 3: Only the receptionist who assigned it can remove it
        /// ALGORITHM: Verify AssignedByStaffID matches before deletion
        /// </summary>
        public async Task<bool> RemoveAssignmentAsync(int assignmentId, int currentStaffId)
        {
            try
            {
                using SqlConnection connection = new(_connectionString);

                // Verify current staff is the one who created it
                string verifyQuery = @"
                    SELECT AssignedByStaffID FROM RoomAssignment 
                    WHERE AssignmentID = @AssignmentID AND IsActive = 1";

                using (SqlCommand verifyCmd = new(verifyQuery, connection))
                {
                    verifyCmd.Parameters.AddWithValue("@AssignmentID", assignmentId);
                    await connection.OpenAsync();

                    object result = await verifyCmd.ExecuteScalarAsync();

                    if (result == null)
                    {
                        Debug.WriteLine($"❌ Assignment {assignmentId} not found");
                        return false;
                    }

                    int assignedByStaffId = (int)result;
                    if (assignedByStaffId != currentStaffId)
                    {
                        Debug.WriteLine($"❌ Only the receptionist who assigned this (Staff {assignedByStaffId}) can remove it");
                        return false;
                    }
                }

                // Soft delete assignment
                string deleteQuery = "UPDATE RoomAssignment SET IsActive = 0 WHERE AssignmentID = @AssignmentID";

                using (SqlCommand deleteCmd = new(deleteQuery, connection))
                {
                    deleteCmd.Parameters.AddWithValue("@AssignmentID", assignmentId);
                    int rowsAffected = await deleteCmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        Debug.WriteLine($"✅ Assignment {assignmentId} removed by staff {currentStaffId}");
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ RemoveAssignmentAsync error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets count of rooms assigned to a nurse on a specific date
        /// ALGORITHM: Simple COUNT with date and active status filters
        /// </summary>
        public async Task<int> GetNurseRoomCountAsync(int nurseId, DateTime assignmentDate)
        {
            try
            {
                using SqlConnection connection = new(_connectionString);

                string query = @"
                    SELECT COUNT(*) FROM RoomAssignment 
                    WHERE NurseID = @NurseID 
                    AND CAST(AssignmentDate AS DATE) = @AssignmentDate 
                    AND IsActive = 1";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@NurseID", nurseId);
                command.Parameters.AddWithValue("@AssignmentDate", assignmentDate.Date);

                await connection.OpenAsync();
                int count = (int)await command.ExecuteScalarAsync();

                Debug.WriteLine($"✅ Nurse {nurseId} has {count} rooms on {assignmentDate:yyyy-MM-dd}");
                return count;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ GetNurseRoomCountAsync error: {ex.Message}");
                return -1;
            }
        }
    }
}