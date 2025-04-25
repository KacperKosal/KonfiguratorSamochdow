using KonfiguratorSamochodowy.Api.Enums;
using Microsoft.AspNetCore.Mvc;

namespace KonfiguratorSamochodowy.Api.Requests;

internal class GetModelsRequest
{
    [FromQuery]
    public string? ModelName { get; set; }

    [FromQuery]
    public SortingTag? SortingTag { get; set; }

    [FromQuery]
    public int? MinPrice { get; set; }

    [FromQuery]
    public int? MaxPrice { get; set; }

    [FromQuery]
    public bool? Has4x4 { get; set; }

    [FromQuery]
    public bool? IsElectrick { get; set; }
}
