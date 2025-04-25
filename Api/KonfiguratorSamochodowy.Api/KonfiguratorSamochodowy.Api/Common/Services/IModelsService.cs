using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Common.Services;

internal interface IModelsService
{
    Task<IEnumerable<ModelDto>> GetModelsAsync(GetModelsRequest request, CancellationToken cancellationToken);

    Task<byte[]> GetModelImageAsync(int id, CancellationToken cancellationToken);
}
