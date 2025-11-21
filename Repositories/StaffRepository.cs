using Hospital_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

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
                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                string query = $"select * from Staff where StaffID in (select StaffOrDoctorID from User_Account where UserID={id})";
                SqlCommand sqlCommand = new(query, connection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    staffData = new Staff
                    {
                        StaffId = reader.GetInt32(0),
                        StaffCode = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Role = reader.GetString(4),
                        DepartmentId = reader.GetInt32(5),
                        Phone = reader.GetString(6),
                        Email = reader.GetString(7),
                        IsActive = reader.GetBoolean(9)
                    };
                    connection.Close();
                    return await Task.FromResult(staffData);
                }
                else
                {
                    connection.Close();
                    return null;
                }
            }
            catch(Exception e) {
                Console.WriteLine("Error:" + e.Message);
                return null;
            }
        }
     }   
}
