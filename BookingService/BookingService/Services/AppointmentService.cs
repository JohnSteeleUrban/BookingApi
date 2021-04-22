using BookingService.DAL;
using BookingService.Models;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppointmentContext _appointmentContext;

        public AppointmentService(AppointmentContext context)
        {
            _appointmentContext = context;
        }

        public async Task<AppointmentDto> AddAppointmentAsync(AppointmentDto appointment)
        {
            //do validations against required parameters
            if (!IsValid(appointment, true))
            {
                return null;
            }

            appointment.Id = Guid.NewGuid();
            var created = await _appointmentContext.CreateAsync(appointment);
            if (!created)
            {
                return null;
            }

            return await GetAppointmentAsync(appointment.Id);
        }

        public async Task<AppointmentDto> GetAppointmentAsync(Guid id)
        {
            return await _appointmentContext.Appointments.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> UpdateAppointmentAsync(AppointmentDto appointment)
        {
            //do validations against required parameters
            if (!IsValid(appointment))
            {
                return false;
            }

            return await _appointmentContext.UpdateAsync(appointment);
        }

        public async Task<bool> CancelAsync(Guid id, string reason)
        {
            //doing this in the context because this space is only for 
            //business rules and validation.  I prefer to 
            //leave all the dal stuff in the context.
            return await _appointmentContext.CancelAsync(id, reason);
        }

        public async Task<List<AppointmentDto>> SearchAppointmentsAsync(string key = "", string filter = "", int index = 0, int count = 50, string order = "")
        {

            //do any other business listing validations or logic applications here

            List<Appointment> appointments =
                await _appointmentContext.FindAppointmentsAsync(key, filter, index, count, order);

            return appointments.Select(a => (AppointmentDto)a).ToList();
        }

        #region Validations

        private bool IsValid(AppointmentDto appointment, bool isCreation = false)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment));
            }

            if (isCreation && appointment.Id == Guid.Empty)
            {
                throw new ArgumentNullException("id");
            }

            if (string.IsNullOrWhiteSpace(appointment.Name))
            {
                throw new ArgumentNullException("name");
            }

            //this vet only takes appointments a day in advance. 
            //there would be more robust limitations added here for business hours
            //or even to check to make sure the type of appointment and duration 
            //coincide with what is expected.

            if (appointment.Start < DateTime.UtcNow.AddDays(1))
            {
                throw new ArgumentException("start: must give at least 1 days notice");
            }

            //of course more checks and business rules can be enforced here.
            //just keeping it simple for this exercise
            if (appointment.End < appointment.Start)
            {
                throw new ArgumentException("end:  must end after start.");
            }

            //as per the requirements, wellness and grooming appointments are 30 mins
            if (appointment.Type == AppointmentType.Wellness || appointment.Type == AppointmentType.Grooming)
            {
                var diff = appointment.End - appointment.Start;
                if ((int)diff.TotalMinutes != 30)
                {
                    throw new ArgumentException($"type: {appointment.Type} Wellness and Grooming must be 30 mins.");
                }
            }

            //as per the requirements, surgery and dental appointments are 60 mins
            if (appointment.Type == AppointmentType.Dental || appointment.Type == AppointmentType.Grooming)
            {
                var diff = appointment.End - appointment.Start;
                if ((int)diff.TotalMinutes != 60)
                {
                    throw new ArgumentException($"type: {appointment.Type} and must be 60 mins.");
                }
            }

            return true;
        }

        #endregion Validations
    }
}
