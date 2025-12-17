using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Hospital_Management_System.Models;
using System.Diagnostics;


namespace Hospital_Management_System.Repositories
{
    public class LoginRepository
    {
        private readonly string _connectionString;
        public LoginRepository(DatabaseConfig dbconfig)
        {
            _connectionString = dbconfig.ConnectionString;
        }
        
        public async Task<UserAccount> GetUserData(string userID)
        {
            UserAccount user = new UserAccount();

            try
            {
               using SqlConnection connect = new(_connectionString);
               await connect.OpenAsync();
               string query = $"select * from User_Account where User_Account.Email='{userID}';";
               using SqlCommand command = new(query, connect);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                
                if(!(await reader.ReadAsync()))
                {
                    return null;
                }
                else
                {
                    user.DocOrStaffId = reader.GetInt32(0);
                    user.Username = reader.GetString(1)+" "+reader.GetString(2);
                    user.Role = reader.GetString(3);
                    user.DepartmentName = reader.GetString(4);
                    user.Email = reader.GetString(5);
                    user.Password = reader.GetString(6);
                    connect.Close();
                    return user;
                }    
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
            
        }

    }
}
