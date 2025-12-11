using Hospital_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace Hospital_Management_System.Repositories
{
    public class StaffRepository
    {
        string _connectionString;
        public StaffRepository(DatabaseConfig dbConfig) {
            _connectionString = dbConfig.ConnectionString;
        }
        public async Task<Staff> GetStaffData(int id)
        {
            Staff staffData;
            try
            {
                using SqlConnection connection = new(_connectionString);
                connection.Open();
                string query = $"select * from Staff where StaffID ={id}";
                using SqlCommand sqlCommand = new(query, connection);
                using SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    staffData = new Staff();
                    staffData.FirstName = reader.GetString(1);
                    staffData.StaffID = reader.GetInt32(0);
                    staffData.LastName = reader.GetString(2);
                    staffData.Role = reader.GetString(3);
                    staffData.Phone = reader.GetString(4);
                    staffData.Email = reader.GetString(5);
                    staffData.HireDate = reader.GetDateTime(6);
                    staffData.IsActive = reader.GetBoolean(7);
                    staffData.Password = reader.GetString(8);
                    staffData.Salary = reader.GetDouble(9);
                    staffData.Gender = reader.GetString(10);
                    staffData.DepartmentName = reader.GetString(11);
                    return staffData;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e) {
                Debug.WriteLine("Error:" + e.Message);
                return null;
            }
        }
        //public async Task<bool> UpdateUsernameAsync(string staffId, string newUsername)
        //{
        //    try
        //    {

        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            await connection.OpenAsync();
        //            // Update the username
        //            string updateQuery = "UPDATE Staff SET Username = @Username WHERE StaffID = @StaffID";
        //            using (var command = new SqlCommand(updateQuery, connection))
        //            {
        //                command.Parameters.AddWithValue("@Username", newUsername);
        //                command.Parameters.AddWithValue("@StaffID", staffId);

        //                int rowsAffected = await command.ExecuteNonQueryAsync();
        //                bool success = rowsAffected > 0;

        //                Console.WriteLine($"📊 UpdateUsername rows affected: {rowsAffected} - {(success ? "✅ Success" : "❌ Failed")}");
        //                return success;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"❌ UpdateUsername ERROR: {ex.Message}");
        //        return false;
        //    }
        //}

        
        public async Task<bool> UpdatePasswordAsync(string staffId, string password)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    // Update the password hash
                    string updateQuery = "UPDATE Staff SET Password = @Password WHERE StaffID = @StaffID";
                    using (var command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@StaffID", staffId);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        bool success = rowsAffected > 0;
                        Console.WriteLine($"📊 StorePasswordHash rows affected: {rowsAffected} - {(success ? "✅ Success" : "❌ Failed")}");
                        return success;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ StorePasswordHash ERROR: {ex.Message}");
                return false;
            }
        }




    }
}
