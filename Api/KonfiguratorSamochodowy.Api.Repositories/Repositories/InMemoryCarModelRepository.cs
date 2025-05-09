using KonfiguratorSamochodowy.Api.Common;
using KonfiguratorSamochodowy.Api.Models;
using KonfiguratorSamochodowy.Api.Requests;
using System.Collections.Concurrent;

namespace KonfiguratorSamochodowy.Api.Repositories
{
    public class InMemoryCarModelRepository : ICarModelRepository
    {
        private static readonly ConcurrentDictionary<string, CarModel> _carModels = new();
        
        static InMemoryCarModelRepository()
        {
            InitializeCarModels();
        }
        
        private static void InitializeCarModels()
        {
            var carModels = new List<CarModel>
            {
                new CarModel
                {
                    Id = "1",
                    Name = "BMW 3 Series",
                    ProductionYear = 2023,
                    BodyType = "Sedan",
                    Manufacturer = "BMW",
                    Segment = "D",
                    BasePrice = 180000,
                    Description = "Sportowy sedan z eleganckimi liniami nadwozia",
                    ImageUrl = "/images/models/bmw-3-series.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-60)
                },
                new CarModel
                {
                    Id = "2",
                    Name = "BMW 5 Series",
                    ProductionYear = 2023,
                    BodyType = "Sedan",
                    Manufacturer = "BMW",
                    Segment = "E",
                    BasePrice = 250000,
                    Description = "Luksusowy sedan klasy średniej-wyższej",
                    ImageUrl = "/images/models/bmw-5-series.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-45)
                },
                new CarModel
                {
                    Id = "3",
                    Name = "BMW X5",
                    ProductionYear = 2023,
                    BodyType = "SUV",
                    Manufacturer = "BMW",
                    Segment = "SUV",
                    BasePrice = 320000,
                    Description = "Luksusowy SUV dla wymagających",
                    ImageUrl = "/images/models/bmw-x5.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new CarModel
                {
                    Id = "4",
                    Name = "BMW i4",
                    ProductionYear = 2023,
                    BodyType = "Sedan",
                    Manufacturer = "BMW",
                    Segment = "D",
                    BasePrice = 240000,
                    Description = "Elektryczny sedan o sportowych osiągach",
                    ImageUrl = "/images/models/bmw-i4.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new CarModel
                {
                    Id = "5",
                    Name = "BMW X3",
                    ProductionYear = 2023,
                    BodyType = "SUV",
                    Manufacturer = "BMW",
                    Segment = "SUV",
                    BasePrice = 220000,
                    Description = "Kompaktowy SUV premium",
                    ImageUrl = "/images/models/bmw-x3.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                }
            };
            
            foreach (var carModel in carModels)
            {
                _carModels[carModel.Id] = carModel;
            }
        }
        
        public Task<Result<IEnumerable<CarModel>>> GetAllAsync()
        {
            return Task.FromResult(Result<IEnumerable<CarModel>>.Success(_carModels.Values));
        }

        public Task<Result<CarModel>> GetByIdAsync(string id)
        {
            if (_carModels.TryGetValue(id, out var carModel))
            {
                return Task.FromResult(Result<CarModel>.Success(carModel));
            }

            return Task.FromResult(Result<CarModel>.Failure(
                new Error("NotFound", $"Nie znaleziono modelu samochodu o ID: {id}")));
        }

        public Task<Result<IEnumerable<CarModel>>> GetFilteredAsync(FilterCarModelsRequest filter)
        {
            var query = _carModels.Values.AsEnumerable();

            if (!string.IsNullOrEmpty(filter.Manufacturer))
                query = query.Where(c => c.Manufacturer.Contains(filter.Manufacturer, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filter.BodyType))
                query = query.Where(c => c.BodyType == filter.BodyType);

            if (!string.IsNullOrEmpty(filter.Segment))
                query = query.Where(c => c.Segment == filter.Segment);

            if (filter.MinProductionYear.HasValue)
                query = query.Where(c => c.ProductionYear >= filter.MinProductionYear);

            if (filter.MaxProductionYear.HasValue)
                query = query.Where(c => c.ProductionYear <= filter.MaxProductionYear);

            if (filter.MinPrice.HasValue)
                query = query.Where(c => c.BasePrice >= filter.MinPrice);

            if (filter.MaxPrice.HasValue)
                query = query.Where(c => c.BasePrice <= filter.MaxPrice);

            if (filter.IsActive.HasValue)
                query = query.Where(c => c.IsActive == filter.IsActive);

            return Task.FromResult(Result<IEnumerable<CarModel>>.Success(query));
        }

        public Task<Result<CarModel>> CreateAsync(CarModel carModel)
        {
            carModel.Id = Guid.NewGuid().ToString();
            carModel.CreatedAt = DateTime.UtcNow;
            
            if (_carModels.TryAdd(carModel.Id, carModel))
            {
                return Task.FromResult(Result<CarModel>.Success(carModel));
            }

            return Task.FromResult(Result<CarModel>.Failure(
                new Error("DatabaseError", "Nie udało się dodać modelu samochodu")));
        }

        public async Task<Result<CarModel>> UpdateAsync(string id, CarModel carModel)
        {
            var getResult = await GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<CarModel>.Failure(getResult.Error);

            carModel.Id = id;
            carModel.CreatedAt = getResult.Value.CreatedAt;
            carModel.UpdatedAt = DateTime.UtcNow;
            
            _carModels[id] = carModel;
            
            return Result<CarModel>.Success(carModel);
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            var getResult = await GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<bool>.Failure(getResult.Error);

            if (_carModels.TryRemove(id, out _))
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                new Error("DatabaseError", $"Nie udało się usunąć modelu samochodu o ID: {id}"));
        }
    }
}