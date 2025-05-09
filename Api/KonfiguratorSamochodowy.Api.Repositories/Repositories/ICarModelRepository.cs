using KonfiguratorSamochodowy.Api.Common;
using KonfiguratorSamochodowy.Api.Models;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Repositories
{
    public interface ICarModelRepository
    {
        Task<Result<IEnumerable<CarModel>>> GetAllAsync();
        Task<Result<CarModel>> GetByIdAsync(string id);
        Task<Result<IEnumerable<CarModel>>> GetFilteredAsync(FilterCarModelsRequest filter);
        Task<Result<CarModel>> CreateAsync(CarModel carModel);
        Task<Result<CarModel>> UpdateAsync(string id, CarModel carModel);
        Task<Result<bool>> DeleteAsync(string id);
    }
}