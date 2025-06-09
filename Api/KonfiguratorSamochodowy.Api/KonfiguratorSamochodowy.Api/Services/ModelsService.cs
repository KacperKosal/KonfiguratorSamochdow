using System.Drawing;
using System.Drawing.Imaging;
using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Repositories.Enums;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Services;

internal class ModelsService(IVehicleRepository vehicleRepository) : IModelsService
{
    public async Task<byte[]> GetModelImageAsync(int id, CancellationToken cancellationToken) //Tymczasowo generuje to losowe zdjecie 
    {
        const int width = 800;
        const int height = 600;

        return await Task.Run(() =>
        {
            using var bmp = new Bitmap(width, height);
            var rnd = Random.Shared;

            for (int y = 0; y < height; y++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                for (int x = 0; x < width; x++)
                {
                    var color = Color.FromArgb(
                        rnd.Next(256),
                        rnd.Next(256),
                        rnd.Next(256));
                    bmp.SetPixel(x, y, color);
                }
            }

            using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }, cancellationToken);
    }

    async Task<IEnumerable<ModelDto>> IModelsService.GetModelsAsync(GetModelsRequest request, CancellationToken cancellationToken) => (await vehicleRepository.GetByFiltersAsync(new Repositories.Options.SortingOptions
    {
        ModelName = request.ModelName,
        SortingOption = (SortingOption?)request.SortingTag,
        MinPrice = request.MinPrice,
        MaxPrice = request.MaxPrice,
        Has4x4 = request.Has4x4,
        IsElectrick = request.IsElectrick,
    })).Select(e =>
    {
        var result = new ModelDto
        {
            VechicleID = e.Id,
            Model = e.Model,
            Price = e.Price,
            Description = e.Description,
            ImageUrl = $"/models/image/{e.Id}",
            Engines = e.Engines?.Select(s => $"{s.Capacity} {s.Type} {s.Power}KM"),
            VehicleFeatures = e.Features?.Select(f => f.Feature),
        };

        return result;
    });
}
