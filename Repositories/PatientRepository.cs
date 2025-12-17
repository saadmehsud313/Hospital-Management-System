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

                string query = @"exec InsertPatient  @FirstName, @LastName, @Gender, @Phone, @BloodGroup, 
                             @EmergencyContactName, @EmergencyContactPhone, @CreatedAt, @IsActive, @Age";

                using SqlCommand command = new(query, connect);
                command.Parameters.AddWithValue("@FirstName", patient.FirstName);
                command.Parameters.AddWithValue("@LastName", patient.LastName);
                command.Parameters.AddWithValue("@Gender", patient.Gender);
                command.Parameters.AddWithValue("@Phone", patient.Phone);
                command.Parameters.AddWithValue("@BloodGroup", patient.BloodGroup);
                command.Parameters.AddWithValue("@EmergencyContactName", patient.EmergencyContactName);
                command.Parameters.AddWithValue("@EmergencyContactPhone", patient.EmergencyContactPhone);
                command.Parameters.AddWithValue("@CreatedAt", patient.CreatedAt);
                command.Parameters.AddWithValue("@IsActive", patient.IsActive ? 1 : 0);
                command.Parameters.AddWithValue("@Age", patient.Age);

                await connect.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if(await reader.ReadAsync())
                {
                    bool result = reader.GetInt32(reader.GetOrdinal("Success"))==1;
                    string message = reader.GetString(reader.GetOrdinal("Message"));
                    string MRN = reader.GetString(reader.GetOrdinal("MRN"));
                    int id = reader.GetInt32(reader.GetOrdinal("PatientId"));
                    if(!result)
                    {
                        await Shell.Current.DisplayAlertAsync("Error", message, "OK");
                        return false;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlertAsync("Success", $"Patient added successfully with \n  MRN: {MRN} and \n ID: {id}", "OK");
                        return true;
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlertAsync("Error", "Failed to add patient.", "OK");
                    return false;
                }
            }
            catch (SqlException e)
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
                await Shell.Current.DisplayAlertAsync("Error", $"Error:{e.Message}", "OK");
                return 0;
            }
        }
        public async Task<Patient> GetPatientAsync(int patientId)
        {
            try
            {
                using SqlConnection connect = new(_connectionString);
                await connect.OpenAsync();
                string query = $"SELECT * FROM Patient WHERE PatientId = @PatientId";
                using SqlCommand command = new(query, connect);
                command.Parameters.AddWithValue("@PatientId", patientId);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    Patient patient = new Patient
                    {
                        PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        MRN = reader.GetString(reader.GetOrdinal("MRN")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        Gender = reader.GetString(reader.GetOrdinal("Gender")),
                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                        Phone = reader.GetString(reader.GetOrdinal("Phone")),
                        BloodGroup = reader.GetString(reader.GetOrdinal("BloodGroup")),
                        EmergencyContactName = reader.GetString(reader.GetOrdinal("EmergencyContactName")),
                        EmergencyContactPhone = reader.GetString(reader.GetOrdinal("EmergencyContactPhone")),
                        Age = reader.GetInt32(reader.GetOrdinal("Age"))
                    };
                    return patient;

                }
                else
                {
                    return null;
                }
            }

            catch (Exception e)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Error: {e.Message}", "OK");
                return null;
            }
        }

    }

}