using KonfiguratorSamochodowy.Api.Dtos;
using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;

namespace KonfiguratorSamochodowy.Api.Common.Services;

    public interface ICarInteriorEquipmentService
    {
        Task<Result<IEnumerable<CarInteriorEquipmentDto>>> GetAllAsync();
        Task<Result<CarInteriorEquipmentDto>> GetByIdAsync(string id);
        Task<Result<IEnumerable<CarInteriorEquipmentDto>>> GetByCarIdAsync(string carId);
        Task<Result<IEnumerable<CarInteriorEquipmentDto>>> GetByCarModelAsync(string carModel);
        Task<Result<IEnumerable<CarInteriorEquipmentDto>>> GetByTypeAsync(string type);
        Task<Result<IEnumerable<CarInteriorEquipmentDto>>> GetFilteredAsync(FilterCarInteriorEquipmentRequest request);
        Task<Result<CarInteriorConfigurationDto>> GetFullCarConfigurationAsync(string carId);
        Task<Result<CarInteriorEquipmentDto>> CreateAsync(CreateCarInteriorEquipmentRequest request);
        Task<Result<CarInteriorEquipmentDto>> UpdateAsync(string id, UpdateCarInteriorEquipmentRequest request);
        Task<Result<bool>> DeleteAsync(string id);
    }
