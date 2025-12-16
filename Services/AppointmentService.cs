using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using Hospital_Management_System.Repository;
using Hospital_Management_System.Models;


namespace Hospital_Management_System.Services
{
    public class AppointmentService
    {
        AppointmentRepository _appointmentRepository;

        public AppointmentService(AppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        public async Task<bool> ScheduleAppointment(Appointment appointment)
        {

            appointment.AppointmentID = await _appointmentRepository.GetLastAppointmentIDAsync() + 1;//fetches last appointment id and adds 1 
            return await _appointmentRepository.AddAppointmentAsync(appointment);
            //return true;
        }
        public async Task<List<Appointment>> GetScheduledAppointmentsAsync()
        {
            return await _appointmentRepository.GetScheduledAppointments();
        }
        public async Task<List<Appointment>> GetTodayAppointmentsByDocID(int docID)
        {
            return await _appointmentRepository.GetAppointmentsByDocIDAsync(docID);
        }
        public async Task<List<Appointment>> GetPendingAppointmentsByDocID(int patientID)
        {
            return await _appointmentRepository.GetPendingAppointmentsByDocIDAsync(patientID);
        }
        public async Task<List<Appointment>> GetHistoryAppointmentByDocID(int docID)
        {
            return await _appointmentRepository.GetHistoryAppointmentsByDocIDAsync(docID);
        }
        public async Task<bool> UpdateAppointmentStatusAsync(int appointmentID, string status)
        {
            return await _appointmentRepository.UpdateAppointmentStatusAsync(appointmentID, status);
        }
        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            return true;
            //return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }

    }


}
