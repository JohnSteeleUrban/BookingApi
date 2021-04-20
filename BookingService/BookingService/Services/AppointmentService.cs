using BookingService.DAL;

namespace BookingService.Services
{
    public class AppointmentService /*: IBookingService*/
    {
        private readonly AppointmentContext _bookingContext;

        public AppointmentService(AppointmentContext context)
        {
            _bookingContext = context;
        }

        public void AddAppointment(Appointment appointment)
        {
            _bookingContext.Appointments.Add(appointment);
            _bookingContext.SaveChanges();
        }
    }
}
