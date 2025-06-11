namespace KonfiguratorSamochodowy.Api.Repositories.Models
{
    public class CarModelColor
    {
        public string Id { get; set; } = string.Empty;
        public string CarModelId { get; set; } = string.Empty;
        public string ColorName { get; set; } = string.Empty;
        public int Price { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}