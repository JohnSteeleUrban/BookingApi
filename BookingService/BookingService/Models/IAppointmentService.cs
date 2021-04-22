using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Models
{
    public interface IAppointmentService
    {
        public Task<AppointmentDto> AddAppointmentAsync(AppointmentDto appointment);
        public Task<AppointmentDto> GetAppointmentAsync(Guid id);
        public Task<bool> UpdateAppointmentAsync(AppointmentDto appointment);
        public Task<bool> CancelAsync(Guid id, string reason);

        public Task<List<AppointmentDto>> SearchAppointmentsAsync(string key, string filter, int index, int count, string order);
    }
}
