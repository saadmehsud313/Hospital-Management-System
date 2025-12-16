using System;

namespace Hospital_Management_System.Models
{
    public class Nurse:Staff
    {
        private int _nurseID;
        private int _staffID;
        private int _assignedRoomID;
        private string _shift;
        private string _phone;

        public int NurseID { get => _nurseID; set => _nurseID = value; }
        public int StaffID { get => _staffID; set => _staffID = value; }
        public int AssignedRoomID { get => _assignedRoomID; set => _assignedRoomID = value; }
        public string Shift { get => _shift; set => _shift = value; }
        public string Phone { get => _phone; set => _phone = value; }
    }
}