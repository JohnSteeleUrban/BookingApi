using BookingService.DAL;
using BookingService.Services;

using Microsoft.AspNetCore.Mvc;

using System;

namespace BookingService.Controllers
{
    [Route("booking/api/v1/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentsController(AppointmentService service)
        {
            _appointmentService = service;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                Guid obj = Guid.NewGuid();
                appointment.Id = obj.ToString();
                _appointmentService.AddAppointment(appointment);
                return Ok();
            }
            return BadRequest();
        }
    }
}
