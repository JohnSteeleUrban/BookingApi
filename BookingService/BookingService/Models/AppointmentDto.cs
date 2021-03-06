using Newtonsoft.Json;

using System;

namespace BookingService.Models
{
    public class AppointmentDto
    {
        public AppointmentDto(Guid id, string name, AnimalType animal, DateTime start, DateTime end, AppointmentType type, bool canceled, string cancelReason)
        {
            Id = id;
            Name = name;
            Animal = animal;
            Start = start;
            End = end;
            Type = type;
            Canceled = canceled;
            CancelReason = cancelReason;
        }

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "animal")]
        public AnimalType Animal { get; set; }

        [JsonProperty(PropertyName = "start")]
        public DateTime Start { get; set; }

        [JsonProperty(PropertyName = "end")]
        public DateTime End { get; set; }

        [JsonProperty(PropertyName = "type")]
        public AppointmentType Type { get; set; }

        [JsonProperty(PropertyName = "canceled")]
        public bool Canceled { get; set; } = false;

        [JsonProperty(PropertyName = "canceledReason")]
        public string CancelReason { get; set; } = String.Empty;

    }
}
