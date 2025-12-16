using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Hospital_Management_System.Models;
using Microsoft.Data.SqlClient;

namespace Hospital_Management_System.Repository
{
    public class NurseRepository
    {
        private readonly string _connectionString;

        public NurseRepository(DatabaseConfig dbConfig)
        {
            _connectionString = dbConfig.ConnectionString;
        }

        public async Task<int> GetNurseIDByStaffIdAsync(int staffId)
        {
            try
            {
                using SqlConnection connection = new(_connectionString);
                await connection.OpenAsync();

                string query = "SELECT NurseID FROM Nurse WHERE StaffID = @StaffID";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@StaffID", staffId);

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    int nurseId = reader.GetInt32(0);
                    Debug.WriteLine($"NurseRepository: Found NurseID {nurseId} for StaffID {staffId}");
                    return nurseId;
                }
                else
                {
                    Debug.WriteLine($"NurseRepository: No NurseID found for StaffID {staffId}");
                    return -1;
                }
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine($"NurseRepository SQL Error: {sqlEx.Message}");
                Debug.WriteLine($"SQL Error Number: {sqlEx.Number}");
                return -1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"NurseRepository General Error: {ex.Message}");
                return -1;
            }
        }

        public async Task<Nurse> GetNurseByStaffIdAsync(int staffId)
        {
            try
            {
                using SqlConnection connection = new(_connectionString);
                await connection.OpenAsync();

                string query = @"
                    SELECT NurseID, StaffID, AssignedRoomID, Shift, Phone 
                    FROM Nurse 
                    WHERE StaffID = @StaffID";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@StaffID", staffId);

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    Nurse nurse = new()
                    {
                        NurseID = reader.GetInt32(0),
                        StaffID = reader.GetInt32(1),
                        AssignedRoomID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                        Shift = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Phone = reader.IsDBNull(4) ? "" : reader.GetString(4)
                    };

                    Debug.WriteLine($"NurseRepository: Loaded nurse details for StaffID {staffId}");
                    return nurse;
                }
                else
                {
                    Debug.WriteLine($"NurseRepository: No nurse found for StaffID {staffId}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"NurseRepository GetNurseByStaffIdAsync Error: {ex.Message}");
                return null;
            }
        }
    }
}