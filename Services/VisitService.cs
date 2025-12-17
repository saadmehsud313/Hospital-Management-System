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
            var doc_service = MauiProgram.Services.GetRequiredService<DoctorService>();
            int id =await doc_service.GetDocIDByStaffId(visit.DoctorId);
            visit.DoctorId = id;
            return await _visitRepository.CreateVisitAsync(visit);
        }
    }
}
