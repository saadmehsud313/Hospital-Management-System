using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Hospital_Management_System.Models;
using System.Diagnostics.Metrics;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
namespace Hospital_Management_System.Repository
{
    public class AppointmentRepository
    {
        string _connectionString;
        public AppointmentRepository(DatabaseConfig dbConfig)
        {
            _connectionString = dbConfig.ConnectionString;
        }

        public async Task<int> GetLastAppointmentIDAsync()
        {
            using SqlConnection connection = new(_connectionString);
            string query = "SELECT TOP 1 AppointmentID FROM Appointment ORDER BY AppointmentID DESC";
            await connection.OpenAsync();
            using SqlCommand command = new(query, connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.Read())
            {
                int lastAppointmentID = reader.GetInt32(0);
                return lastAppointmentID;
            }
            else
            {
                return 1000;
            }
        }
        public async Task<bool> AddAppointmentAsync(Appointment appointment)
        {
            try
            {
                using SqlConnection connection = new(_connectionString);

                string query = @"exec sp_InsertAppointment @PatientID,@DoctorID,@CreatedByStaffID,@ScheduledAt,@Reason,@Status";

                using SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@PatientID", appointment.PatientID);
                command.Parameters.AddWithValue("@DoctorID", appointment.DoctorID);
                command.Parameters.AddWithValue("@CreatedByStaffID", appointment.CreatedByStaff);
                command.Parameters.AddWithValue("@ScheduledAt", appointment.AppointmentDate);  // DateTime
                command.Parameters.AddWithValue("@Reason", appointment.Reason);
                command.Parameters.AddWithValue("@Status", appointment.Status);

                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    bool success = reader.GetInt32(reader.GetOrdinal("Success")) == 1;
                    string message = reader.GetString(reader.GetOrdinal("Message"));
                    int appointmentId = reader.IsDBNull(reader.GetOrdinal("AppointmentID")) ? 0 : reader.GetInt32(reader.GetOrdinal("AppointmentID"));
                    DateTime scheduledAt = reader.IsDBNull(reader.GetOrdinal("ScheduledAt")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("ScheduledAt"));
                    if (!success)
                    {
                        await Shell.Current.DisplayAlertAsync("Error", message, "Okay");
                        return false;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlertAsync("Success", $"Appointment ID: {appointmentId} scheduled at {scheduledAt}", "Okay");
                        return true;
                    }
                }
                else
                {
                    return false;
                }
             }
            catch(SqlException ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Error:{ex.Message} occured", "Okay");
                return false;
            }
        }
        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            List<Appointment> appointments = new();
            using SqlConnection connection = new(_connectionString);
            string query = "SELECT AppointmentID, PatientID, DoctorID, CreatedByStaffID, ScheduledAt, Reason, Status FROM Appointment";
            await connection.OpenAsync();
            using SqlCommand command = new(query, connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Appointment appointment = new()
                {
                    AppointmentID = reader.GetInt32(0),
                    PatientID = reader.GetInt32(1),
                    DoctorID = reader.GetInt32(2),
                    CreatedByStaff = reader.GetInt32(3),
                    AppointmentDate = reader.GetDateTime(4),
                    Reason = reader.GetString(5),
                    Status = reader.GetString(6)
                };
                appointments.Add(appointment);
            }
            return appointments;

        }
        public async Task<List<Appointment>> GetAppointmentsByDocIDAsync(int docID)
        {
            List<Appointment> appointments = new();
            using SqlConnection connection = new(_connectionString);
            string query = "exec GetTodayAppointmentByDocID @DoctorID";
            await connection.OpenAsync();
            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@DoctorID", docID);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Debug.WriteLine("Reading appointment record...");
                Appointment appointment = new()
                {
                    AppointmentID = reader.GetInt32(0),
                    PatientID = reader.GetInt32(1),
                    DoctorID = reader.GetInt32(2),
                    CreatedByStaff = reader.GetInt32(3),
                    AppointmentDate = reader.GetDateTime(4),
                    Status = reader.GetString(5),
                    Reason = reader.GetString(6),
                    PatientName = reader.GetString(7),
                    PatientPhone = reader.GetString(8)
                };
                appointments.Add(appointment);
            }
            return appointments;
        }
        public async Task<List<Appointment>> GetPendingAppointmentsByDocIDAsync(int docID)
        {
            try
            {
                List<Appointment> appointments = new();
                using SqlConnection connection = new(_connectionString);
                string query = "exec GetPendingAppointments @DoctorID";
                await connection.OpenAsync();
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@DoctorID", docID);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Debug.WriteLine("Reading appointment record...");
                    Appointment appointment = new()
                    {
                        AppointmentID = reader.GetInt32(0),
                        PatientID = reader.GetInt32(1),
                        DoctorID = reader.GetInt32(2),
                        CreatedByStaff = reader.GetInt32(3),
                        AppointmentDate = reader.GetDateTime(4),
                        Status = reader.GetString(5),
                        Reason = reader.GetString(6),
                        PatientName = reader.GetString(7),
                        PatientPhone = reader.GetString(8)
                    };
                    appointments.Add(appointment);
                }
                    return appointments;
            }
            catch(SqlException ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Error:{ex.Message} occured", "Okay");
                return null;
            }
                
        }
        public async Task<List<Appointment>> GetHistoryAppointmentsByDocIDAsync(int docID)
        {
            var appointments = new List<Appointment>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // FIXED: Use SQL GETDATE() and get past appointments OR completed/cancelled
                    string query = "exec  GetHistoryAppointmentByDocID @DoctorID";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DoctorID", docID);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var appointment = new Appointment
                                {
                                    AppointmentID = Convert.ToInt32(reader["AppointmentID"]),
                                    PatientID = Convert.ToInt32(reader["PatientID"]),
                                    DoctorID = Convert.ToInt32(reader["DoctorID"]),
                                    CreatedByStaff = Convert.ToInt32(reader["CreatedByStaffID"]),
                                    AppointmentDate = Convert.ToDateTime(reader["ScheduledAt"]),
                                    Status = reader["Status"].ToString(),
                                    Reason = reader["Reason"].ToString(),
                                    PatientName = reader["PatientName"].ToString(),
                                    PatientPhone = reader["PatientPhone"].ToString()
                                };

                                appointments.Add(appointment);
                            }

                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Error:{ex.Message} occured", "Okay");
            }

            return appointments;
        }
        public async Task<bool> UpdateAppointmentStatusAsync(int appointmentID, string status)
        {
            try
            {
                using SqlConnection connection = new(_connectionString);
                string query = "UPDATE Appointment SET Status = @Status WHERE AppointmentID = @AppointmentID";
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Status", status);
                command.Parameters.AddWithValue("@AppointmentID", appointmentID);
                await connection.OpenAsync();
                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating appointment status: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Appointment>> GetScheduledAppointments()
        {
            List<Appointment> appointments = new();
            using SqlConnection connection = new(_connectionString);
            string query = "SELECT AppointmentID, PatientID, DoctorID, CreatedByStaffID, ScheduledAt, Reason, Status FROM Appointment Where Status='Scheduled'";
            await connection.OpenAsync();
            using SqlCommand command = new(query, connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Appointment appointment = new()
                {
                    AppointmentID = reader.GetInt32(0),
                    PatientID = reader.GetInt32(1),
                    DoctorID = reader.GetInt32(2),
                    CreatedByStaff = reader.GetInt32(3),
                    AppointmentDate = reader.GetDateTime(4),
                    Reason = reader.GetString(5),
                    Status = reader.GetString(6)
                };
                if (appointment.Status.Equals("Scheduled"))
                { 
                 appointments.Add(appointment);
                }
                
            }
            return appointments;

        }
        public async Task<bool> UpdatAppointmentAsync(Appointment appointment)
        {
            try
            {
                using SqlConnection connection = new(_connectionString);
                string query = @"exec sp_UpdateAppointment @AppointmentID,@PatientID,@DoctorID,@CreatedByStaffID,@ScheduledAt
                                @Reason,@Status";
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AppointmentID", appointment.AppointmentID);
                command.Parameters.AddWithValue("@PatientID", appointment.PatientID);
                command.Parameters.AddWithValue("@DoctorID", appointment.DoctorID);
                command.Parameters.AddWithValue("@CreatedByStaffID", appointment.CreatedByStaff);
                command.Parameters.AddWithValue("@ScheduledAt", appointment.AppointmentDate);
                command.Parameters.AddWithValue("@Reason", appointment.Reason);
                command.Parameters.AddWithValue("@Status", appointment.Status);
                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if(await reader.ReadAsync())
                {
                    bool success = reader.GetInt32(reader.GetOrdinal("Success")) == 1;
                    string message = reader.GetString(reader.GetOrdinal("Message"));
                    DateTime scheduledAt=reader.GetDateTime(reader.GetOrdinal("ScheduledAt"));

                    if (!success)
                    {
                        await Shell.Current.DisplayAlertAsync("Error", message, "Okay");
                        return false;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlertAsync("Success",$"{message}\n New Appointment  Scheduled At:{scheduledAt}", "Okay");
                        return true;
                     }
                }
                return true;
            }
            catch{
                return false;
            }
        }
    }
}

