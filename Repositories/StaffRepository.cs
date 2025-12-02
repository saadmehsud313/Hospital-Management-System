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
            catch(Exception e) {
                Debug.WriteLine("Error:" + e.Message);
                return null;
            }
        }
     }   
}
