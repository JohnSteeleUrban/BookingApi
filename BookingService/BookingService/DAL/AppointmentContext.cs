using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace BookingService.DAL
{
    public class AppointmentContext : DbContext
    {
        public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options) 
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Postgres has a default schema of public
            //otherwise the default is usually dbo
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Appointment> Appointments { get; set; }

        public async Task<bool> CreateAsync(Appointment appointment)
        {
            this.Appointments.Add(appointment);
            var result = await this.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateAsync(Appointment appointment)
        {
            var match = await Appointments.FirstOrDefaultAsync(a => a.Id == appointment.Id);

            bool updated = false;

            if (match != null)
            {
                match.Name = appointment.Name;
                match.Animal = appointment.Animal;
                match.Start = appointment.Start;
                match.End = appointment.End;
                match.Type = appointment.Type;
                match.Created = appointment.Created;
                match.LastModified = DateTime.UtcNow;

                var result = await SaveChangesAsync();
                updated = result >= 0;
            }

            return updated;

        }

        public async Task<bool> CancelAsync(Guid id, string reason)
        {
            var updated = false;

            var match = await Appointments.FirstOrDefaultAsync(a => a.Id == id);
            if (match != null)
            {
                match.Canceled = true;
                match.CancelReason = reason;
                var result = await SaveChangesAsync();
                updated = result >= 0;
            }

            return updated;
        }
    }
}
