using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BookingService.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AppointmentType
    {
        [EnumMember(Value = "wellness")]
        Wellness = 0,

        [EnumMember(Value = "surgery")]
        Surgery,

        [EnumMember(Value = "grooming")]
        Grooming,

        [EnumMember(Value = "dental")]
        Dental,

        [EnumMember(Value = "other")]
        Other
    }
}
