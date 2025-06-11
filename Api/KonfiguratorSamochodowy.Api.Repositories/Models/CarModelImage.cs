namespace KonfiguratorSamochodowy.Api.Repositories.Models
{
    public class CarModelImage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CarModelId { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsMainImage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}