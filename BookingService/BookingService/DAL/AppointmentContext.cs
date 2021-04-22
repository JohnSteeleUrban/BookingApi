using BookingService.Extensions;
using BookingService.Models;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.DAL
{
    /// <summary>
    /// The appointments Context - connection to the database.
    /// we are assuming all data coming into this class has been validated by the business layer -
    /// or at least that's the intent, I may have missed some spots :-)
    /// </summary>
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
#if DEBUG
            #region AppointmentSeed
            modelBuilder.Entity<Appointment>().HasData(new Appointment { Id = Guid.NewGuid(), Name = "Garry Catsparov", Animal = AnimalType.Cat, Start = DateTime.UtcNow.AddDays(2) , End = DateTime.UtcNow.AddDays(2).AddMinutes(30) , Type = AppointmentType.Wellness});
            modelBuilder.Entity<Appointment>().HasData(new Appointment { Id = Guid.NewGuid(), Name = "Carl Winslow", Animal = AnimalType.Bird, Start = DateTime.UtcNow.AddDays(2), End = DateTime.UtcNow.AddDays(2).AddMinutes(30), Type = AppointmentType.Grooming });
            modelBuilder.Entity<Appointment>().HasData(new Appointment { Id = Guid.NewGuid(), Name = "Tweety", Animal = AnimalType.Bird, Start = DateTime.UtcNow.AddDays(2), End = DateTime.UtcNow.AddDays(2).AddMinutes(60), Type = AppointmentType.Dental });
            modelBuilder.Entity<Appointment>().HasData(new Appointment { Id = Guid.NewGuid(), Name = "Goofy", Animal = AnimalType.Dog, Start = DateTime.UtcNow.AddDays(2), End = DateTime.UtcNow.AddDays(2).AddMinutes(60), Type = AppointmentType.Surgery });
            modelBuilder.Entity<Appointment>().HasData(new Appointment { Id = Guid.NewGuid(), Name = "Oscar Wildecatt", Animal = AnimalType.Cat, Start = DateTime.UtcNow.AddDays(2), End = DateTime.UtcNow.AddDays(2).AddMinutes(30), Type = AppointmentType.Other });
            modelBuilder.Entity<Appointment>().HasData(new Appointment { Id = Guid.NewGuid(), Name = "Miss Piggy", Animal = AnimalType.Other, Start = DateTime.UtcNow.AddDays(2), End = DateTime.UtcNow.AddDays(2).AddMinutes(30), Type = AppointmentType.Wellness });
            modelBuilder.Entity<Appointment>().HasData(new Appointment { Id = Guid.NewGuid(), Name = "Fluffy", Animal = AnimalType.Cat, Start = DateTime.UtcNow.AddDays(2), End = DateTime.UtcNow.AddDays(2).AddMinutes(30), Type = AppointmentType.Wellness });

            #endregion
#endif
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

        public async Task<List<Appointment>> FindAppointmentsAsync(string key = "", string filter = "", int index = 0, int count = 50, string order = "")
        {
            List<Appointment> appointments = await Appointments
                .Filter(key, filter)
                .Order(order)
                .Skip(index)
                .Take(count)
                .ToListAsync();

            return appointments;
        }
    }
}
