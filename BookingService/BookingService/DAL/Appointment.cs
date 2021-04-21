using BookingService.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.DAL
{
    [Table("appointments")]
    public class Appointment
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("animal")]
        public AnimalType Animal { get; set; }

        [Column("start")]
        public DateTime Start { get; set; }

        [Column("end")]
        public DateTime End { get; set; }

        [Column("type") ]
        public AppointmentType Type { get; set; }

        [Column("canceled")] 
        public bool Canceled { get; set; } = false;

        [Column("cancel_reason")]
        public string CancelReason { get; set; }

        [Column("created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [Column("last_modified")]
        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public static implicit operator Appointment(AppointmentDto dto)
        {
            return new Appointment
            {
                Id = dto.Id,
                Name = dto.Name,
                Animal = dto.Animal,
                Start = dto.Start,
                End = dto.End,
                Type = dto.Type
            };
        }

        public static implicit operator AppointmentDto(Appointment dto)
        {
            return new AppointmentDto(dto.Id, dto.Name, dto.Animal, dto.Start, dto.End, dto.Type);
        }

    }
}
