using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BookingService.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AnimalType
    {
        [EnumMember(Value = "dog")]
        Dog = 0,

        [EnumMember(Value = "cat")]
        Cat,

        [EnumMember(Value = "bird")]
        Bird,

        [EnumMember(Value = "other")]
        Other,
    }
}
