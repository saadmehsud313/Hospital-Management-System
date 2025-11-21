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
                SqlConnection connect = new SqlConnection(_connectionString);
                connect.Open();
                String query = $"select * from User_Account where User_Account.UserID={userID};";
                SqlCommand command = new(query, connect);
                SqlDataReader reader = command.ExecuteReader();
                
                if(!(reader.Read()))
                {
                    return null;
                }
                else
                {
                    user.UserId = reader.GetInt32(0);
                    user.Username = reader.GetString(1);
                    user.Password = reader.GetString(2);
                    user.Role = reader.GetString(3);
                    user.DocOrStaffId = reader.GetInt32(4);
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
