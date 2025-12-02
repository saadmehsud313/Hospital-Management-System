using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management_System.Models
{
    public class Doctor
    {
        int _doctorId;
        int _staffID;
        string _firstName;
        string _lastName;
        string _speciality;
        string _qualification;
        string _phone;
        string _email;
        string _departmentName;
        double _consultationFee;
        int _maxAppointments;
        bool _isActive;
        public int DoctorId { get => _doctorId; set => _doctorId = value; }
        public int StaffID { get => _staffID; set => _staffID = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string Speciality { get => _speciality; set => _speciality = value; }
        public string Qualification { get => _qualification; set => _qualification = value; }
        public string Phone { get => _phone; set => _phone = value; }
        public string Email { get => _email; set => _email = value; }
        public string DepartmentName { get => _departmentName; set => _departmentName = value; }
        public double ConsultationFee { get => _consultationFee; set => _consultationFee = value; }
        public bool IsActive { get => _isActive; set => _isActive = value; }
        public int MaxAppointments { get => _maxAppointments; set => _maxAppointments = value; }
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

    }
}
