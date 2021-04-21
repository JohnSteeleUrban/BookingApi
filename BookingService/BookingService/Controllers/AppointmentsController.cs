using BookingService.Models;
using BookingService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BookingService.Controllers
{
    [Route("booking/api/v1/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;
        private readonly ILogger _logger;

        public AppointmentsController(AppointmentService service, ILogger<AppointmentsController> logger)
        {
            _appointmentService = service;
            _logger = logger;

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AppointmentDto appointment)
        {
            ActionResult result = StatusCode((int)HttpStatusCode.InternalServerError, "The content could not be displayed because an internal server error has occured.");

            try
            {
                var newAppointment = await _appointmentService.AddAppointmentAsync(appointment);

                //by design, returns null if validation fails and doesn't throw, 
                //or context fails to create without error.
                if (newAppointment == null)
                {
                    throw new InvalidOperationException($"Could not be created.");
                }

                result = StatusCode((int) HttpStatusCode.Created, newAppointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> Get(Guid id)
        {
            AppointmentDto appointment;

            try
            {
                appointment = await _appointmentService.GetAppointmentAsync(id);
                if (appointment == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

            return appointment;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] AppointmentDto appointment)
        {
            ActionResult result;

            try
            {
                if (appointment == null)
                {
                    throw new ArgumentNullException(nameof(appointment));
                }

                var currentAppointment = await _appointmentService.GetAppointmentAsync(id);
                //if the appointment does not exist
                if (currentAppointment == null)
                {
                    return NotFound();
                }

                bool updated = await _appointmentService.UpdateAppointmentAsync(appointment);

                if (!updated)
                {
                    throw new InvalidOperationException($"The Appointment could not be updated (Id: {appointment.Id:D}).");
                }

                result = Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

            return result;
        }

        [HttpPut("{id}/{reason}")]
        public async Task<IActionResult> CancelAsync(Guid id, string reason)
        {
            ActionResult result;
            try
            {
                var appointment = await _appointmentService.GetAppointmentAsync(id);
                //does it exist?
                if (appointment == null)
                {
                    return NotFound();
                }

                if (string.IsNullOrWhiteSpace(reason))
                {
                    throw new ArgumentNullException(nameof(reason));
                }

                bool canceled = await _appointmentService.CancelAsync(id, reason);

                if (!canceled)
                {
                    throw new InvalidOperationException($"The Appointment could not be canceled (Id: {appointment.Id:D}).");
                }

                result = Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

            return result;
        }
    }
}
