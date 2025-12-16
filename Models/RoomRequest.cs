using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital_Management_System.Models
{
    public class RoomRequest
    {
        int _requestId;
        int _patientId;
        int _doctorId;
        int _respondedByStaffId;
        string _status;
        public int RequestId { get => _requestId; set => _requestId = value; }
        public int PatientId { get => _patientId; set => _patientId = value; }
        public int DoctorId { get => _doctorId; set => _doctorId = value; }
        public int RespondedByStaffId { get => _respondedByStaffId; set => _respondedByStaffId = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
