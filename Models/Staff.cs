using System;

namespace Hospital_Management_System.Models
{
    public class Staff
    {
        private int _staffId;
        private string _firstName;
        private string _lastName;
        private string _fullName;
        private string _gender;
        private double _salary;
        private string _role;
        private string _email;
        private string _phone;
        private string _password;
        private DateTime _hireDate;
        private string _departmentName;
        private bool _isActive;

        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public int StaffID { get => _staffId; set => _staffId = value; }
        public string Gender { get => _gender; set => _gender = value; }
        public string Phone { get => _phone; set => _phone = value; }
        public string Role { get => _role; set => _role = value; }
        public string Email { get => _email; set => _email = value; }
        public double Salary { get => _salary; set => _salary = value; }
        public bool IsActive { get => _isActive; set => _isActive = value; }
        public string Password { get => _password; set => _password = value; }
        public DateTime HireDate { get => _hireDate; set => _hireDate = value; }
        public string DepartmentName { get => _departmentName; set => _departmentName = value; }
        public string FullName
        {
            get => $"{FirstName} {LastName}";
         }
    }
}