using Hospital_Management_System.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management_System.Repository
{
    public class PatientRepository
    {
        private readonly DatabaseConfig _databaseConfig;
        private readonly string _connectionString;
        public PatientRepository(DatabaseConfig databaseConfig)
        {
            _connectionString = databaseConfig.ConnectionString;
        }

        public async Task<bool> AddPatientAsync(Patient patient)
        {
            try
            {
                using SqlConnection connect = new(_connectionString);

                string query = @"
        INSERT INTO PATIENT 
        (PatientId, FirstName, LastName, MRN, Gender, Phone, BloodGroup, 
         EmergencyContactName, EmergencyContactPhone, CreatedAt, IsActive, Age)
        VALUES 
        (@PatientId, @FirstName, @LastName, @MRN, @Gender, @Phone, @BloodGroup, 
         @EmergencyContactName, @EmergencyContactPhone, @CreatedAt, @IsActive, @Age)";

                using SqlCommand command = new(query, connect);

                command.Parameters.AddWithValue("@PatientId", patient.PatientId);
                command.Parameters.AddWithValue("@FirstName", patient.FirstName);
                command.Parameters.AddWithValue("@LastName", patient.LastName);
                command.Parameters.AddWithValue("@MRN", patient.MRN);
                command.Parameters.AddWithValue("@Gender", patient.Gender);
                command.Parameters.AddWithValue("@Phone", patient.Phone);
                command.Parameters.AddWithValue("@BloodGroup", patient.BloodGroup);
                command.Parameters.AddWithValue("@EmergencyContactName", patient.EmergencyContactName);
                command.Parameters.AddWithValue("@EmergencyContactPhone", patient.EmergencyContactPhone);
                command.Parameters.AddWithValue("@CreatedAt", patient.CreatedAt);
                command.Parameters.AddWithValue("@IsActive", patient.IsActive ? 1 : 0);
                command.Parameters.AddWithValue("@Age", patient.Age);

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

        public async Task<int> GetLastPatientAsync()
        {
            try
            {
                using SqlConnection connect = new(_connectionString);
                await connect.OpenAsync();
                string query = $"select top 1 PatientId from Patient order by PatientID Desc";
                using SqlCommand command = new(query, connect);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    Debug.WriteLine(reader.GetInt32(0));
                    return reader.GetInt32(0);
                    
                }
                else
                {
                    return 1001;
                }
            }
            catch (Exception e)
             {
                await  Shell.Current.DisplayAlertAsync("Error",$"Error:{e.Message}", "OK");
                return 0;
            }
        }

    }
}
