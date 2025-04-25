using KonfiguratorSamochodowy.Api.Repositories.Enums;

namespace KonfiguratorSamochodowy.Api.Repositories.Options;

public class SortingOptions
{
    public string? ModelName { get; set; }

    public SortingOption? SortingOption { get; set; }

    public int? MinPrice { get; set; }
    
    public int? MaxPrice { get; set; }

    public bool? Has4x4 { get; set; }

    public bool? IsElectrick { get; set; }
}
