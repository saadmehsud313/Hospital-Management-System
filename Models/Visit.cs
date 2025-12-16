using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital_Management_System.Models
{
    public class Visit
    {
        int _visitId;
        int _doctorId;
        int _patientId;
        int _appointmentId;
        string _diagnosisSummary;
        string _prescriptions;
        string _symptoms;
        string _visitType;
        DateTime _visitDateTime;
        DateTime _CreatedAt;
        DateTime? _followUpDate;

        public int VisitId { get => _visitId; set => _visitId = value; }
        public int DoctorId { get => _doctorId; set => _doctorId = value; }
        public int PatientId { get => _patientId; set => _patientId = value; }
        public string DiagnosisSummary { get => _diagnosisSummary; set => _diagnosisSummary = value; }
        public string Prescriptions { get => _prescriptions; set => _prescriptions = value; }
        public string Symptoms { get => _symptoms; set => _symptoms = value; }
        public string VisitType { get => _visitType; set => _visitType = value; }
        public DateTime VisitDateTime { get => _visitDateTime; set => _visitDateTime = value; }
        public DateTime CreatedAt { get => _CreatedAt; set => _CreatedAt = value; }
        public DateTime? FollowUpDate { get => _followUpDate; set => _followUpDate = value; }
        public int AppointmentId { get => _appointmentId; set => _appointmentId = value; }
    }
}
