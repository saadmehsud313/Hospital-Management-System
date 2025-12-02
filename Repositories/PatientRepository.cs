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

        public  async Task AddPatientAsync(Patient patient)
        {
            Debug.WriteLineIf(true, "Adding Patient to Database");
            Debug.WriteLine($"Firs Name:{patient.FirstName} ID Generated:{patient.PatientId}");
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
