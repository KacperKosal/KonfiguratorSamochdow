using KonfiguratorSamochodowy.Api.Repositories.Dto;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using System.Collections.Concurrent;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories
{
    public class InMemoryEngineRepository : IEngineRepository
    {
        private static readonly ConcurrentDictionary<string, Engine> _engines = new();
        
        static InMemoryEngineRepository()
        {
            InitializeEngines();
        }
        
        private static void InitializeEngines()
        {
            var engines = new List<Engine>
            {
                new Engine
                {
                    Id = "1",
                    Name = "2.0 TwinPower Turbo",
                    Type = "Petrol",
                    Capacity = 1998,
                    Power = 184,
                    Torque = 290,
                    FuelType = "Benzyna",
                    Cylinders = 4,
                    Transmission = "Automatyczna",
                    Gears = 8,
                    DriveType = "RWD",
                    FuelConsumption = 6.8m,
                    CO2Emission = 155,
                    Description = "Ekonomiczny silnik benzynowy z technologią TwinPower Turbo",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-60)
                },
                new Engine
                {
                    Id = "2",
                    Name = "3.0 TwinPower Turbo",
                    Type = "Petrol",
                    Capacity = 2998,
                    Power = 286,
                    Torque = 450,
                    FuelType = "Benzyna",
                    Cylinders = 6,
                    Transmission = "Automatyczna",
                    Gears = 8,
                    DriveType = "RWD",
                    FuelConsumption = 7.6m,
                    CO2Emission = 172,
                    Description = "Mocny 6-cylindrowy silnik benzynowy o wysokich osiągach",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-50)
                },
                new Engine
                {
                    Id = "3",
                    Name = "2.0 TwinPower Turbo Diesel",
                    Type = "Diesel",
                    Capacity = 1995,
                    Power = 190,
                    Torque = 400,
                    FuelType = "Diesel",
                    Cylinders = 4,
                    Transmission = "Automatyczna",
                    Gears = 8,
                    DriveType = "RWD",
                    FuelConsumption = 5.2m,
                    CO2Emission = 136,
                    Description = "Wydajny silnik wysokoprężny z niskim zużyciem paliwa",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-45)
                },
                new Engine
                {
                    Id = "4",
                    Name = "3.0 TwinPower Turbo Diesel",
                    Type = "Diesel",
                    Capacity = 2993,
                    Power = 265,
                    Torque = 620,
                    FuelType = "Diesel",
                    Cylinders = 6,
                    Transmission = "Automatyczna",
                    Gears = 8,
                    DriveType = "AWD",
                    FuelConsumption = 6.4m,
                    CO2Emission = 168,
                    Description = "Mocny silnik Diesel z technologią TwinPower Turbo i napędem na wszystkie koła",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-40)
                },
                new Engine
                {
                    Id = "5",
                    Name = "Electric Drive",
                    Type = "Electric",
                    Capacity = null,
                    Power = 340,
                    Torque = 430,
                    FuelType = "Elektryczny",
                    Cylinders = null,
                    Transmission = "Automatyczna",
                    Gears = 1,
                    DriveType = "RWD",
                    FuelConsumption = 0,
                    CO2Emission = 0,
                    Description = "Napęd elektryczny o wysokiej mocy i zerowej emisji CO2",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Engine
                {
                    Id = "6",
                    Name = "2.0 TwinPower Turbo PHEV",
                    Type = "PlugInHybrid",
                    Capacity = 1998,
                    Power = 292,
                    Torque = 420,
                    FuelType = "Benzyna/Elektryczny",
                    Cylinders = 4,
                    Transmission = "Automatyczna",
                    Gears = 8,
                    DriveType = "AWD",
                    FuelConsumption = 1.9m,
                    CO2Emission = 44,
                    Description = "Hybryda Plug-In z możliwością ładowania z zewnętrznych źródeł",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                }
            };
            
            foreach (var engine in engines)
            {
                _engines[engine.Id] = engine;
            }
        }
        
        public Task<Result<IEnumerable<Engine>>> GetAllAsync()
        {
            return Task.FromResult(Result<IEnumerable<Engine>>.Success(_engines.Values));
        }

        public Task<Result<Engine>> GetByIdAsync(string id)
        {
            if (_engines.TryGetValue(id, out var engine))
            {
                return Task.FromResult(Result<Engine>.Success(engine));
            }

            return Task.FromResult(Result<Engine>.Failure(
                new Error("NotFound", $"Nie znaleziono silnika o ID: {id}")));
        }

        public Task<Result<IEnumerable<Engine>>> GetFilteredAsync(FilterEnginesRequestDto filter)
        {
            var query = _engines.Values.AsEnumerable();

            if (!string.IsNullOrEmpty(filter.Type))
                query = query.Where(e => e.Type == filter.Type);

            if (!string.IsNullOrEmpty(filter.FuelType))
                query = query.Where(e => e.FuelType.Contains(filter.FuelType, StringComparison.OrdinalIgnoreCase));

            if (filter.MinCapacity.HasValue)
                query = query.Where(e => e.Capacity.HasValue && e.Capacity.Value >= filter.MinCapacity.Value);

            if (filter.MaxCapacity.HasValue)
                query = query.Where(e => e.Capacity.HasValue && e.Capacity.Value <= filter.MaxCapacity.Value);

            if (filter.MinPower.HasValue)
                query = query.Where(e => e.Power >= filter.MinPower.Value);

            if (filter.MaxPower.HasValue)
                query = query.Where(e => e.Power <= filter.MaxPower.Value);

            if (!string.IsNullOrEmpty(filter.Transmission))
                query = query.Where(e => e.Transmission == filter.Transmission);

            if (!string.IsNullOrEmpty(filter.DriveType))
                query = query.Where(e => e.DriveType == filter.DriveType);

            if (filter.IsActive.HasValue)
                query = query.Where(e => e.IsActive == filter.IsActive);

            return Task.FromResult(Result<IEnumerable<Engine>>.Success(query));
        }

        public Task<Result<Engine>> CreateAsync(Engine engine)
        {
            engine.Id = Guid.NewGuid().ToString();
            engine.CreatedAt = DateTime.UtcNow;
            
            if (_engines.TryAdd(engine.Id, engine))
            {
                return Task.FromResult(Result<Engine>.Success(engine));
            }

            return Task.FromResult(Result<Engine>.Failure(
                new Error("DatabaseError", "Nie udało się dodać silnika")));
        }

        public async Task<Result<Engine>> UpdateAsync(string id, Engine engine)
        {
            var getResult = await GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<Engine>.Failure(getResult.Error);

            engine.Id = id;
            engine.CreatedAt = getResult.Value.CreatedAt;
            engine.UpdatedAt = DateTime.UtcNow;
            
            _engines[id] = engine;
            
            return Result<Engine>.Success(engine);
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            var getResult = await GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<bool>.Failure(getResult.Error);

            if (_engines.TryRemove(id, out _))
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                new Error("DatabaseError", $"Nie udało się usunąć silnika o ID: {id}"));
        }

        public Task<IEnumerable<Engine>> GetAllByVechicleIdAsync(int vehicleId)
        {
            // Filter engines by vehicle ID - for in-memory implementation, we'll use a simple filter
            // that matches engines where the ID matches the vehicleId parameter
            var result = _engines.Values
                .Where(e => e.ID == vehicleId || int.TryParse(e.Id, out var id) && id == vehicleId)
                .ToList();
            
            return Task.FromResult<IEnumerable<Engine>>(result);
        }
    }
}