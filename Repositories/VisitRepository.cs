using Hospital_Management_System.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Hospital_Management_System.Repositories
{
    public class VisitRepository
    {
        string _connectionString;
        public VisitRepository(DatabaseConfig dbconfig)
        {
            _connectionString = dbconfig.ConnectionString;
        }


        public async Task<bool> CreateVisitAsync(Visit visit)
        {
            try
            {
                using SqlConnection connect = new(_connectionString);
                string query = @"
                            INSERT INTO VISIT 
                            (VisitId, DoctorId, PatientId, DiagnosisSummary, Prescription, Symptoms, 
                             VisitType, VisitDateTime, CreatedAt, FollowUpDate)
                            VALUES 
                            (@VisitId, @DoctorId, @PatientId, @DiagnosisSummary, @Prescription, @Symptoms, 
                             @VisitType, @VisitDateTime, @CreatedAt, @FollowUpDate)";
                using SqlCommand command = new(query, connect);
                command.Parameters.AddWithValue("@VisitId", visit.VisitId);
                command.Parameters.AddWithValue("@DoctorId", visit.DoctorId);
                command.Parameters.AddWithValue("@PatientId", visit.PatientId);
                command.Parameters.AddWithValue("@DiagnosisSummary", visit.DiagnosisSummary);
                command.Parameters.AddWithValue("@Prescription", visit.Prescriptions);
                command.Parameters.AddWithValue("@Symptoms", visit.Symptoms);
                command.Parameters.AddWithValue("@VisitType", visit.VisitType);
                command.Parameters.AddWithValue("@VisitDateTime", visit.VisitDateTime);
                command.Parameters.AddWithValue("@CreatedAt", visit.CreatedAt);
                command.Parameters.AddWithValue("@FollowUpDate", (object?)visit.FollowUpDate ?? DBNull.Value);
                await connect.OpenAsync();
                int affectedRows = await command.ExecuteNonQueryAsync();
                return affectedRows > 0;
            }
            catch (Exception e)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Error: {e.Message}", "OK");
                return false;
            }
        }
        public async Task<int> GetLastVisitAsync()
        {
            try
            {
                using SqlConnection connect = new(_connectionString);
                await connect.OpenAsync();
                string query = $"select top 1 VisitId from Visit order by VisitID Desc";
                using SqlCommand command = new(query, connect);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    Debug.WriteLine(reader.GetInt32(0));
                    return reader.GetInt32(0);
                }
                else
                {
                    return 6001;
                }
            }
            catch (Exception e)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Error:{e.Message}", "OK");
                return 0;
            }
        }
    }
}
