using System;

namespace Hospital_Management_System.Models
{
    public class RoomAssignment
    {
        private int _assignmentID;
        private int _nurseID;
        private int _roomID;
        private DateTime _assignmentDate;
        private int _assignedByStaffID;
        private DateTime _createdAt;
        private bool _isActive;
        private string _roomNumber;
        private string _ward;
        private string _roomType;
        private int _patientID;
        private string _patientName;
        private string _admitReason;
        private int _admissionID;

        public int AssignmentID { get => _assignmentID; set => _assignmentID = value; }
        public int NurseID { get => _nurseID; set => _nurseID = value; }
        public int RoomID { get => _roomID; set => _roomID = value; }
        public DateTime AssignmentDate { get => _assignmentDate; set => _assignmentDate = value; }
        public int AssignedByStaffID { get => _assignedByStaffID; set => _assignedByStaffID = value; }
        public DateTime CreatedAt { get => _createdAt; set => _createdAt = value; }
        public bool IsActive { get => _isActive; set => _isActive = value; }
        public string RoomNumber { get => _roomNumber; set => _roomNumber = value; }
        public string Ward { get => _ward; set => _ward = value; }
        public string RoomType { get => _roomType; set => _roomType = value; }
        public int PatientID { get => _patientID; set => _patientID = value; }
        public string PatientName { get => _patientName; set => _patientName = value; }
        public string AdmitReason { get => _admitReason; set => _admitReason = value; }
        public int AdmissionID { get => _admissionID; set => _admissionID = value; }
    }
}