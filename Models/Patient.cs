using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management_System.Models
{
    public class Patient
    {
        int _patientId;
        string? _mrn;
        string? _firstName;
        string? _lastName;
        string? _dateOfBirth;
        string? _Gender;
        string? _phone;
        string? _email;
        string? _address;
        string? _BloodGroup;
        string? _emergencyContactName;
        string? _emergencyContactPhone;
        DateTime? _createdAt = null;
        bool _isActive;
        int age;

        public int PatientId { get => _patientId; set => _patientId = value; }
        public string MRN { get => _mrn; set => _mrn = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string DateOfBirth { get => _dateOfBirth; set => _dateOfBirth = value; }
        public string Gender { get => _dateOfBirth; set=> _dateOfBirth = value; }
        public string Phone { get => _phone; set => _phone = value; }
        public string BloodGroup { get => _BloodGroup; set => _BloodGroup = value; }
        public string EmergencyContactName { get => _emergencyContactName; set => _emergencyContactName = value; }
        public string EmergencyContactPhone { get => _emergencyContactPhone; set => _emergencyContactPhone = value; }
        public DateTime? CreatedAt { get => _createdAt; set => _createdAt = value; }
        public bool IsActive { get => _isActive; set => _isActive = value; }
        public int Age { get => age; set => age = value; }


    }
}
