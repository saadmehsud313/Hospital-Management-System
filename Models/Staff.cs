using System;


namespace Hospital_Management_System.Models
{
    public class Staff
    {
        int _staffId;
        string _staffCode;
        string _firstName;
        string _lastName;
        string _role;
        int _departmentId;
        string _phone;
        string _email;
        string _hireDate;
        bool _isActive;
        public int StaffId { get => _staffId; set => _staffId = value; }
        public string StaffCode { get => _staffCode; set => _staffCode = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string Role { get => _role; set => _role = value; }
        public int DepartmentId { get => _departmentId; set => _departmentId = value; }
        public string Phone { get => _phone; set => _phone = value; }
        public string Email { get => _email; set => _email = value; }
        public string HireDate { get => HireDate; set => HireDate = value; }
        public bool IsActive { get => _isActive; set => _isActive = value; }
    }
}
