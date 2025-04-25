using System.Text.Json.Serialization;

namespace KonfiguratorSamochodowy.Api.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum SortingTag
{
    PriceAsc, 
    PriceDesc,
    Name,
}
