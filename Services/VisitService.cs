using Hospital_Management_System.Models;
using Hospital_Management_System.Repositories;
using Hospital_Management_System.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital_Management_System.Services
{
    public class VisitService
    {
        private readonly VisitRepository _visitRepository;
        public VisitService()
        {
            _visitRepository = MauiProgram.Services.GetRequiredService<VisitRepository>();
        }
        public async Task<bool> CreateVisit(Visit visit)
        {
            visit.VisitId = await _visitRepository.GetLastVisitAsync() + 1;
            visit.CreatedAt = DateTime.Now;
            var _docService = MauiProgram.Services.GetRequiredService<DoctorService>();
            int id= await _docService.GetDocIDByStaffId(visit.DoctorId);
            visit.DoctorId = id;
            var _appointmentRepository = MauiProgram.Services.GetRequiredService<AppointmentRepository>();
            bool visStatus = await _visitRepository.CreateVisitAsync(visit);
            bool appStatus= await _appointmentRepository.UpdateAppointmentStatusAsync(visit.AppointmentId, "Completed");
            return appStatus && visStatus;
        }
    }
}
