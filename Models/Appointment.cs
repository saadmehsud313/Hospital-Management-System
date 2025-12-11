using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital_Management_System.Models
{
    public class Appointment
    {
        private int _appointmentID;
        private int _patientID;
        private int _doctorID;
        private int _createdByStaff;
        private DateTime _appointmentDate;
        private string _status;
        private string _reason;
        private int _durationMinutes;
        private string _patientName;
        private string _patientPhone;
        public int AppointmentID { get => _appointmentID; set => _appointmentID = value; }
        public int PatientID { get => _patientID; set => _patientID = value; }
        public int DoctorID { get => _doctorID; set => _doctorID = value; }
        public int CreatedByStaff { get => _createdByStaff; set => _createdByStaff = value; }
        public DateTime AppointmentDate { get => _appointmentDate; set => _appointmentDate = value; }
        public string Status { get => _status; set => _status = value; }
        public string Reason { get => _reason; set => _reason = value; }
        public int DurationMinutes { get => _durationMinutes; set => _durationMinutes = value; }
        public string PatientName { get => _patientName; set => _patientName = value; }
        public string PatientPhone { get => _patientPhone; set => _patientPhone = value; }
    }
}
