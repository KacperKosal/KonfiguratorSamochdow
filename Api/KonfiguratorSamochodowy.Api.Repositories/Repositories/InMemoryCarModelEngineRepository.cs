using KonfiguratorSamochodowy.Api.Models;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories
{
    public class InMemoryCarModelEngineRepository : ICarModelEngineRepository
    {
        private static readonly ConcurrentDictionary<string, CarModelEngine> _carModelEngines = new();
        
        static InMemoryCarModelEngineRepository()
        {
            InitializeCarModelEngines();
        }
        
        private static void InitializeCarModelEngines()
        {
            var carModelEngines = new List<CarModelEngine>
            {
                // BMW 3 Series z silnikami
                new CarModelEngine
                {
                    Id = "1",
                    CarModelId = "1", // BMW 3 Series
                    EngineId = "1",   // 2.0 TwinPower Turbo
                    AdditionalPrice = 0,
                    IsDefault = true,
                    TopSpeed = 235,
                    Acceleration0To100 = 7.1m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-60)
                },
                new CarModelEngine
                {
                    Id = "2",
                    CarModelId = "1", // BMW 3 Series
                    EngineId = "3",   // 2.0 TwinPower Turbo Diesel
                    AdditionalPrice = 5000,
                    IsDefault = false,
                    TopSpeed = 230,
                    Acceleration0To100 = 6.8m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-59)
                },
                new CarModelEngine
                {
                    Id = "3",
                    CarModelId = "1", // BMW 3 Series
                    EngineId = "6",   // 2.0 TwinPower Turbo PHEV
                    AdditionalPrice = 25000,
                    IsDefault = false,
                    TopSpeed = 225,
                    Acceleration0To100 = 5.8m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-58)
                },
                
                // BMW 5 Series z silnikami
                new CarModelEngine
                {
                    Id = "4",
                    CarModelId = "2", // BMW 5 Series
                    EngineId = "1",   // 2.0 TwinPower Turbo
                    AdditionalPrice = 0,
                    IsDefault = true,
                    TopSpeed = 235,
                    Acceleration0To100 = 7.5m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-50)
                },
                new CarModelEngine
                {
                    Id = "5",
                    CarModelId = "2", // BMW 5 Series
                    EngineId = "2",   // 3.0 TwinPower Turbo
                    AdditionalPrice = 25000,
                    IsDefault = false,
                    TopSpeed = 250,
                    Acceleration0To100 = 5.2m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-49)
                },
                new CarModelEngine
                {
                    Id = "6",
                    CarModelId = "2", // BMW 5 Series
                    EngineId = "3",   // 2.0 TwinPower Turbo Diesel
                    AdditionalPrice = 5000,
                    IsDefault = false,
                    TopSpeed = 232,
                    Acceleration0To100 = 7.3m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-48)
                },
                new CarModelEngine
                {
                    Id = "7",
                    CarModelId = "2", // BMW 5 Series
                    EngineId = "4",   // 3.0 TwinPower Turbo Diesel
                    AdditionalPrice = 30000,
                    IsDefault = false,
                    TopSpeed = 250,
                    Acceleration0To100 = 5.7m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-47)
                },
                
                // BMW X5 z silnikami
                new CarModelEngine
                {
                    Id = "8",
                    CarModelId = "3", // BMW X5
                    EngineId = "2",   // 3.0 TwinPower Turbo
                    AdditionalPrice = 0,
                    IsDefault = true,
                    TopSpeed = 243,
                    Acceleration0To100 = 5.5m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-40)
                },
                new CarModelEngine
                {
                    Id = "9",
                    CarModelId = "3", // BMW X5
                    EngineId = "4",   // 3.0 TwinPower Turbo Diesel
                    AdditionalPrice = 5000,
                    IsDefault = false,
                    TopSpeed = 230,
                    Acceleration0To100 = 5.7m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-39)
                },
                new CarModelEngine
                {
                    Id = "10",
                    CarModelId = "3", // BMW X5
                    EngineId = "6",   // 2.0 TwinPower Turbo PHEV
                    AdditionalPrice = 35000,
                    IsDefault = false,
                    TopSpeed = 235,
                    Acceleration0To100 = 5.6m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-38)
                },
                
                // BMW i4 z silnikiem elektrycznym
                new CarModelEngine
                {
                    Id = "11",
                    CarModelId = "4", // BMW i4
                    EngineId = "5",   // Electric Drive
                    AdditionalPrice = 0,
                    IsDefault = true,
                    TopSpeed = 200,
                    Acceleration0To100 = 5.7m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                
                // BMW X3 z silnikami
                new CarModelEngine
                {
                    Id = "12",
                    CarModelId = "5", // BMW X3
                    EngineId = "1",   // 2.0 TwinPower Turbo
                    AdditionalPrice = 0,
                    IsDefault = true,
                    TopSpeed = 215,
                    Acceleration0To100 = 8.2m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new CarModelEngine
                {
                    Id = "13",
                    CarModelId = "5", // BMW X3
                    EngineId = "3",   // 2.0 TwinPower Turbo Diesel
                    AdditionalPrice = 5000,
                    IsDefault = false,
                    TopSpeed = 213,
                    Acceleration0To100 = 7.9m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-19)
                },
                new CarModelEngine
                {
                    Id = "14",
                    CarModelId = "5", // BMW X3
                    EngineId = "6",   // 2.0 TwinPower Turbo PHEV
                    AdditionalPrice = 25000,
                    IsDefault = false,
                    TopSpeed = 210,
                    Acceleration0To100 = 6.1m,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-18)
                }
            };
            
            foreach (var carModelEngine in carModelEngines)
            {
                _carModelEngines[carModelEngine.Id] = carModelEngine;
            }
        }
        
        public Task<Result<IEnumerable<CarModelEngine>>> GetAllAsync()
        {
            return Task.FromResult(Result<IEnumerable<CarModelEngine>>.Success(_carModelEngines.Values));
        }

        public Task<Result<CarModelEngine>> GetByIdAsync(string id)
        {
            if (_carModelEngines.TryGetValue(id, out var carModelEngine))
            {
                return Task.FromResult(Result<CarModelEngine>.Success(carModelEngine));
            }

            return Task.FromResult(Result<CarModelEngine>.Failure(
                new Error("NotFound", $"Nie znaleziono powiązania model-silnik o ID: {id}")));
        }

        public Task<Result<CarModelEngine>> GetByCarModelAndEngineIdAsync(string carModelId, string engineId)
        {
            var carModelEngine = _carModelEngines.Values
                .FirstOrDefault(cme => cme.CarModelId == carModelId && cme.EngineId == engineId);
                
            if (carModelEngine != null)
            {
                return Task.FromResult(Result<CarModelEngine>.Success(carModelEngine));
            }

            return Task.FromResult(Result<CarModelEngine>.Failure(
                new Error("NotFound", $"Nie znaleziono powiązania dla modelu {carModelId} i silnika {engineId}")));
        }

        public Task<Result<IEnumerable<CarModelEngine>>> GetByCarModelIdAsync(string carModelId)
        {
            var carModelEngines = _carModelEngines.Values
                .Where(cme => cme.CarModelId == carModelId)
                .ToList();
                
            return Task.FromResult(Result<IEnumerable<CarModelEngine>>.Success(carModelEngines));
        }

        public Task<Result<IEnumerable<CarModelEngine>>> GetByEngineIdAsync(string engineId)
        {
            var carModelEngines = _carModelEngines.Values
                .Where(cme => cme.EngineId == engineId)
                .ToList();
                
            return Task.FromResult(Result<IEnumerable<CarModelEngine>>.Success(carModelEngines));
        }

        public Task<Result<CarModelEngine>> CreateAsync(CarModelEngine carModelEngine)
        {
            carModelEngine.Id = Guid.NewGuid().ToString();
            carModelEngine.CreatedAt = DateTime.UtcNow;
            
            if (_carModelEngines.TryAdd(carModelEngine.Id, carModelEngine))
            {
                return Task.FromResult(Result<CarModelEngine>.Success(carModelEngine));
            }

            return Task.FromResult(Result<CarModelEngine>.Failure(
                new Error("DatabaseError", "Nie udało się dodać powiązania model-silnik")));
        }

        public async Task<Result<CarModelEngine>> UpdateAsync(string id, CarModelEngine carModelEngine)
        {
            var getResult = await GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<CarModelEngine>.Failure(getResult.Error);

            carModelEngine.Id = id;
            carModelEngine.CreatedAt = getResult.Value.CreatedAt;
            carModelEngine.UpdatedAt = DateTime.UtcNow;
            
            _carModelEngines[id] = carModelEngine;
            
            return Result<CarModelEngine>.Success(carModelEngine);
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            var getResult = await GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<bool>.Failure(getResult.Error);

            if (_carModelEngines.TryRemove(id, out _))
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                new Error("DatabaseError", $"Nie udało się usunąć powiązania model-silnik o ID: {id}"));
        }

        public async Task<Result<bool>> DeleteByCarModelAndEngineIdAsync(string carModelId, string engineId)
        {
            var getResult = await GetByCarModelAndEngineIdAsync(carModelId, engineId);
            if (!getResult.IsSuccess)
                return Result<bool>.Failure(getResult.Error);

            return await DeleteAsync(getResult.Value.Id);
        }
    }
}