using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Hospital_Management_System.Models;
using System.Diagnostics.Metrics;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

                string query = @"
            INSERT INTO Appointment 
            (AppointmentID, PatientID, DoctorID, CreatedByStaffID, ScheduledAt, Reason, Status)
            VALUES
            (@AppointmentID, @PatientID, @DoctorID, @CreatedByStaffID, @ScheduledAt, @Reason, @Status);
        ";

                using SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@AppointmentID", appointment.AppointmentID);
                command.Parameters.AddWithValue("@PatientID", appointment.PatientID);
                command.Parameters.AddWithValue("@DoctorID", appointment.DoctorID);
                command.Parameters.AddWithValue("@CreatedByStaffID", appointment.CreatedByStaff);
                command.Parameters.AddWithValue("@ScheduledAt", appointment.AppointmentDate);  // DateTime
                command.Parameters.AddWithValue("@Reason", appointment.Reason);
                command.Parameters.AddWithValue("@Status", appointment.Status);

                await connection.OpenAsync();
                int rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Error: {ex.Message}", "OK");
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
        public async Task<List<Appointment>> GetHistoryAppointmentsByDocIDAsync(int docID)
        {
            var appointments = new List<Appointment>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // FIXED: Use SQL GETDATE() and get past appointments OR completed/cancelled
                    string query = "exec  GetPendingAppointmentsByDocID @DoctorID";

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
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR in GetAppointmentHistory: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
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
    }
}

