using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.DAL
{
    [Table("appointments")]
    public class Appointment
    {
        [Column("id")]
        public string Id { get; set; }
        public string name { get; set; }
    }
}
