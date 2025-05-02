using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;

namespace KonfiguratorSamochodowy.Api.Repositories.Interfaces;

    public interface ICarInteriorEquipmentRepository
    {
        Task<Result<IEnumerable<CarInteriorEquipment>>> GetAllAsync();
        Task<Result<CarInteriorEquipment>> GetByIdAsync(string id);
        Task<Result<IEnumerable<CarInteriorEquipment>>> GetByCarIdAsync(string carId);
        Task<Result<IEnumerable<CarInteriorEquipment>>> GetByCarModelAsync(string carModel);
        Task<Result<IEnumerable<CarInteriorEquipment>>> GetByTypeAsync(string type);
        Task<Result<IEnumerable<CarInteriorEquipment>>> GetFilteredAsync(string? carId = null, string? carModel = null, string? type = null, bool? isDefault = null, decimal? maxPrice = null);
        Task<Result<CarInteriorEquipment>> CreateAsync(CarInteriorEquipment equipment);
        Task<Result<CarInteriorEquipment>> UpdateAsync(string id, CarInteriorEquipment equipment);
        Task<Result<bool>> DeleteAsync(string id);
    }
