using Hospital_Management_System.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital_Management_System.Repositories
{
    public class AdmissionRepository
    {
        private readonly string _connectionString;

        public AdmissionRepository(DatabaseConfig databseConfig)
        {
            _connectionString = databseConfig.ConnectionString;
        }
        public async Task<bool> CreateAdmitRequest(RoomRequest roomRequest)
        {
            try 
            { 
                SqlConnection connection = new SqlConnection(_connectionString);
                string query = @"exec sp_CreateAdmitRequest @PatientID,@DoctorID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PatientID", roomRequest.PatientId);
                command.Parameters.AddWithValue("@DoctorID", roomRequest.DoctorId);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if(await reader.ReadAsync())
                {
                    bool result = reader.GetInt32(reader.GetOrdinal("Success"))==1;
                    string message = reader.GetString(reader.GetOrdinal("Message"));
                    if(!result)
                    {
                        await Shell.Current.DisplayAlertAsync("Error", message, "OK");
                        return false;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlertAsync("Success", message, "OK");
                        return true;
                    }
                }
            }
            catch (SqlException e)
            {
                await Shell.Current.DisplayAlertAsync("Error", e.Message, "OK");
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateAdmitRequest(RoomRequest roomRequest)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                string query = @"exec sp_CreateAdmitRequest @RequestID,@RespondedByStaffID,@Staff";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RequestID", roomRequest.RequestId);
                command.Parameters.AddWithValue("@RespondedByStaffID", roomRequest.RespondedByStaffId);
                command.Parameters.AddWithValue("@Staff", roomRequest.Status);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    bool result = reader.GetInt32(reader.GetOrdinal("Success")) == 1;
                    string message = reader.GetString(reader.GetOrdinal("Message"));
                    if (!result)
                    {
                        await Shell.Current.DisplayAlertAsync("Error", message, "OK");
                        return false;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlertAsync("Success", message, "OK");
                        return true;
                    }
                }
            }
            catch (SqlException e)
            {
                await Shell.Current.DisplayAlertAsync("Error", e.Message, "OK");
                return false;
            }
            return true;
        }
    }
}
