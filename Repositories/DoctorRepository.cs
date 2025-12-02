using Hospital_Management_System.Models;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management_System.Repositories
{
    public class DoctorRepository
    {
        private  readonly  string _connectionString;
        public DoctorRepository(DatabaseConfig config)
        {
            _connectionString = config.ConnectionString;
        }
        public  List<Doctor> GetAllDOctors()
        {
            List<Doctor> doctors = new List<Doctor>();
            using SqlConnection connect = new SqlConnection(_connectionString);
            string sqlQuery = "Select * from DoctorView";
            using SqlCommand command = new SqlCommand(sqlQuery, connect);
            connect.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Doctor doctor = new Doctor();
                {
                    doctor.StaffID = reader.GetInt32(0);
                    doctor.DoctorId = reader.GetInt32(1);
                    doctor.FirstName = reader.GetString(2);
                    doctor.LastName = reader.GetString(3);
                    doctor.Speciality = reader.GetString(5);
                    doctor.MaxAppointments = reader.GetInt32(6);
                    doctor.ConsultationFee =reader.GetDouble(7);
                }
                doctors.Add(doctor);
                
            }
            return doctors;
        }


    }
}
