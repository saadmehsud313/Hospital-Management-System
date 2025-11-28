using Hospital_Management_System.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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

        public async void AddPatient(Patient patient)
        {
            try
            {
                SqlConnection connect = new(_connectionString);
                connect.Open();
                string query = $"insert into ";
            }
            catch
            {

            }
        }
        public async Task<int> GetLastPatient()
        {
            // Code to get the last patient from the database using _connectionString
            return await Task.FromResult(0);
        }

    }
}
