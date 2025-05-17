using KonfiguratorSamochodowy.Api.Repositories.Enums;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using System.Collections.Concurrent;


namespace KonfiguratorSamochodowy.Api.Repositories.Repositories
{
    public class InMemoryCarAccessoryRepository : ICarAccessoryRepository
    {
        private static readonly ConcurrentDictionary<string, CarAccessory> _accessories = new();

        static InMemoryCarAccessoryRepository()
        {
            InitializeBMWAccessories();
        }

        private static void InitializeBMWAccessories()
        {
            // BMW X5
            var bmwX5Id = "1";
            var bmwX5Model = "X5";

            // Felgi
            AddAccessory(new CarAccessory
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Category = AccessoryCategory.Wheels,
                Type = AccessoryType.AlloyWheels,
                Name = "BMW M Double-spoke 748 M",
                Description = "Lekkie felgi aluminiowe M, 21 cali",
                Price = 8500,
                Manufacturer = "BMW",
                PartNumber = "36112459594",
                IsOriginalBMWPart = true,
                IsInStock = true,
                StockQuantity = 4,
                Size = "21\"10",
                Pattern = "Double-spoke",
                Color = "Jet Black",
                InstallationDifficulty = "Professional",
                Warranty = "2 lata",
                ImageUrl = "/images/wheels/748m-21.jpg"
            });

            AddAccessory(new CarAccessory
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Category = AccessoryCategory.Wheels,
                Type = AccessoryType.AlloyWheels,
                Name = "BMW V-spoke 734",
                Description = "Stylowe felgi aluminiowe, 20 cali",
                Price = 6200,
                Manufacturer = "BMW",
                PartNumber = "36112459593",
                IsOriginalBMWPart = true,
                IsInStock = true,
                StockQuantity = 8,
                Size = "20\"9",
                Pattern = "V-spoke",
                Color = "Orbit Grey",
                InstallationDifficulty = "Professional",
                Warranty = "2 lata",
                ImageUrl = "/images/wheels/734-20.jpg"
            });

            // Przyciemniane szyby
            AddAccessory(new CarAccessory
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Category = AccessoryCategory.Exterior,
                Type = AccessoryType.TintedWindows,
                Name = "Przyciemniane szyby tylne",
                Description = "Profesjonalne przyciemnienie szyb tylnych, stopień przyciemnienia 25%",
                Price = 1200,
                Manufacturer = "3M",
                PartNumber = "BMW-TINT-X5-25",
                IsOriginalBMWPart = false,
                IsInStock = true,
                StockQuantity = 999,
                Color = "Dark",
                InstallationDifficulty = "Professional",
                Warranty = "5 lat",
                ImageUrl = "/images/exterior/tinted-windows.jpg"
            });

            // Dywaniki
            AddAccessory(new CarAccessory
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Category = AccessoryCategory.Interior,
                Type = AccessoryType.FloorMats,
                Name = "Dywaniki welurowe BMW X5",
                Description = "Oryginalne dywaniki welurowe z logo BMW, antypoślizgowe",
                Price = 650,
                Manufacturer = "BMW",
                PartNumber = "51472450517",
                IsOriginalBMWPart = true,
                IsInStock = true,
                StockQuantity = 15,
                Color = "Black",
                Material = "Welur",
                InstallationDifficulty = "Easy",
                Warranty = "2 lata",
                ImageUrl = "/images/interior/floor-mats-velour.jpg"
            });

            AddAccessory(new CarAccessory
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Category = AccessoryCategory.Interior,
                Type = AccessoryType.FloorMats,
                Name = "Dywaniki gumowe BMW X5",
                Description = "Wszechsezonowe dywaniki gumowe, łatwe do czyszczenia",
                Price = 420,
                Manufacturer = "BMW",
                PartNumber = "51472458551",
                IsOriginalBMWPart = true,
                IsInStock = true,
                StockQuantity = 20,
                Color = "Black",
                Material = "Guma",
                InstallationDifficulty = "Easy",
                Warranty = "2 lata",
                ImageUrl = "/images/interior/floor-mats-rubber.jpg"
            });

            // Bagażnik dachowy
            AddAccessory(new CarAccessory
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Category = AccessoryCategory.Transport,
                Type = AccessoryType.RoofBoxes,
                Name = "BMW Roof Box 420",
                Description = "Aerodynamiczny bagażnik dachowy, 420 litrów",
                Price = 2800,
                Manufacturer = "BMW",
                PartNumber = "82732406459",
                IsOriginalBMWPart = true,
                IsInStock = true,
                StockQuantity = 5,
                Color = "Black",
                Capacity = 420,
                MaxLoad = 75,
                InstallationDifficulty = "Medium",
                Warranty = "3 lata",
                ImageUrl = "/images/transport/roof-box-420.jpg"
            });

            // Uchwyt na rowery
            AddAccessory(new CarAccessory
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Category = AccessoryCategory.Transport,
                Type = AccessoryType.BikeCarriers,
                Name = "BMW Bike Carrier Pro 2.0",
                Description = "Profesjonalny uchwyt na 2 rowery, montowany na haku",
                Price = 2200,
                Manufacturer = "BMW",
                PartNumber = "82722326514",
                IsOriginalBMWPart = true,
                IsInStock = true,
                StockQuantity = 8,
                MaxLoad = 60,
                InstallationDifficulty = "Medium",
                Warranty = "2 lata",
                ImageUrl = "/images/transport/bike-carrier.jpg"
            });

            // Ładowarka indukcyjna
            AddAccessory(new CarAccessory
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Category = AccessoryCategory.Electronics,
                Type = AccessoryType.WirelessChargers,
                Name = "BMW Wireless Charging Station",
                Description = "Ładowarka indukcyjna do telefonu, moc 15W",
                Price = 650,
                Manufacturer = "BMW",
                PartNumber = "61252458830",
                IsOriginalBMWPart = true,
                IsInStock = true,
                StockQuantity = 30,
                Capacity = 15,
                Compatibility = "Qi Standard",
                InstallationDifficulty = "Professional",
                Warranty = "2 lata",
                ImageUrl = "/images/electronics/wireless-charger.jpg"
            });

            // Kamera cofania
            AddAccessory(new CarAccessory
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Category = AccessoryCategory.Safety,
                Type = AccessoryType.RearViewCameras,
                Name = "BMW Advanced Rearview Camera",
                Description = "Kamera cofania HD z dynamicznymi liniami pomocniczymi",
                Price = 1800,
                Manufacturer = "BMW",
                PartNumber = "66212458836",
                IsOriginalBMWPart = true,
                IsInStock = true,
                StockQuantity = 10,
                InstallationDifficulty = "Professional",
                Warranty = "2 lata",
                ImageUrl = "/images/safety/rearview-camera.jpg"
            });

            // Łańcuchy śniegowe
            AddAccessory(new CarAccessory
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Category = AccessoryCategory.Seasonal,
                Type = AccessoryType.SnowChains,
                Name = "BMW Snow Chains Comfort",
                Description = "Łańcuchy śniegowe Comfort, rozmiar dla kół 20-21\"",
                Price = 580,
                Manufacturer = "RUD",
                PartNumber = "36112296312",
                IsOriginalBMWPart = true,
                IsInStock = true,
                StockQuantity = 12,
                Size = "20-21\"",
                InstallationDifficulty = "Medium",
                Warranty = "1 rok",
                ImageUrl = "/images/seasonal/snow-chains.jpg"
            });

            // Apple CarPlay
            AddAccessory(new CarAccessory
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Category = AccessoryCategory.Connectivity,
                Type = AccessoryType.AppleCarPlay,
                Name = "Apple CarPlay Preparation",
                Description = "Przygotowanie pod Apple CarPlay, włączając bezprzewodowe połączenie",
                Price = 1500,
                Manufacturer = "BMW",
                PartNumber = "65502465045",
                IsOriginalBMWPart = true,
                IsInStock = true,
                StockQuantity = 999,
                Compatibility = "iPhone 5 and newer",
                InstallationDifficulty = "Professional",
                Warranty = "2 lata",
                ImageUrl = "/images/connectivity/apple-carplay.jpg"
            });

            // Fotelik dziecięcy
            AddAccessory(new CarAccessory
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Category = AccessoryCategory.Family,
                Type = AccessoryType.ChildSeats,
                Name = "BMW Junior Seat 2-3",
                Description = "Fotelik dziecięcy dla dzieci 15-36 kg, grupa 2-3",
                Price = 980,
                Manufacturer = "BMW",
                PartNumber = "82222348233",
                IsOriginalBMWPart = true,
                IsInStock = true,
                StockQuantity = 8,
                AgeGroup = "3-12 lat",
                MaxLoad = 36,
                Color = "Black/Blue",
                InstallationDifficulty = "Easy",
                Warranty = "2 lata",
                ImageUrl = "/images/family/child-seat.jpg"
            });

            // BMW 3 Series
            var bmw3Id = "2";
            var bmw3Model = "3 Series";

            
            AddAccessory(new CarAccessory
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmw3Id,
                CarModel = bmw3Model,
                Category = AccessoryCategory.Wheels,
                Type = AccessoryType.AlloyWheels,
                Name = "BMW M Double-spoke 791 M",
                Description = "Sportowe felgi aluminiowe M, 19 cali",
                Price = 5800,
                Manufacturer = "BMW",
                PartNumber = "36112459548",
                IsOriginalBMWPart = true,
                IsInStock = true,
                StockQuantity = 4,
                Size = "19\"8.5",
                Pattern = "Double-spoke",
                Color = "Bicolor",
                InstallationDifficulty = "Professional",
                Warranty = "2 lata",
                ImageUrl = "/images/wheels/791m-19.jpg"
            });

            
        }

        private static void AddAccessory(CarAccessory accessory)
        {
            _accessories[accessory.Id] = accessory;
        }

        public Task<Result<IEnumerable<CarAccessory>>> GetAllAsync()
        {
            return Task.FromResult(Result<IEnumerable<CarAccessory>>.Success(_accessories.Values));
        }

        public Task<Result<CarAccessory>> GetByIdAsync(string id)
        {
            if (_accessories.TryGetValue(id, out var accessory))
            {
                return Task.FromResult(Result<CarAccessory>.Success(accessory));
            }

            return Task.FromResult(Result<CarAccessory>.Failure(
                new Error("NotFound", $"Nie znaleziono akcesorium o ID: {id}")));
        }

        public Task<Result<IEnumerable<CarAccessory>>> GetByCarIdAsync(string carId)
        {
            var result = _accessories.Values.Where(a => a.CarId == carId);
            return Task.FromResult(Result<IEnumerable<CarAccessory>>.Success(result));
        }

        public Task<Result<IEnumerable<CarAccessory>>> GetByCarModelAsync(string carModel)
        {
            var result = _accessories.Values.Where(a => a.CarModel == carModel);
            return Task.FromResult(Result<IEnumerable<CarAccessory>>.Success(result));
        }

        public Task<Result<IEnumerable<CarAccessory>>> GetByCategoryAsync(string category)
        {
            var result = _accessories.Values.Where(a => a.Category == category);
            return Task.FromResult(Result<IEnumerable<CarAccessory>>.Success(result));
        }

        public Task<Result<IEnumerable<CarAccessory>>> GetByTypeAsync(string type)
        {
            var result = _accessories.Values.Where(a => a.Type == type);
            return Task.FromResult(Result<IEnumerable<CarAccessory>>.Success(result));
        }

        public Task<Result<IEnumerable<CarAccessory>>> GetFilteredAsync(
            string? carId = null,
            string? carModel = null,
            string? category = null,
            string? type = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            bool? isOriginalBMWPart = null,
            bool? isInStock = null,
            string? installationDifficulty = null)
        {
            var query = _accessories.Values.AsEnumerable();

            if (!string.IsNullOrEmpty(carId))
                query = query.Where(a => a.CarId == carId);

            if (!string.IsNullOrEmpty(carModel))
                query = query.Where(a => a.CarModel == carModel);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(a => a.Category == category);

            if (!string.IsNullOrEmpty(type))
                query = query.Where(a => a.Type == type);

            if (minPrice.HasValue)
                query = query.Where(a => a.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(a => a.Price <= maxPrice.Value);

            if (isOriginalBMWPart.HasValue)
                query = query.Where(a => a.IsOriginalBMWPart == isOriginalBMWPart.Value);

            if (isInStock.HasValue)
                query = query.Where(a => a.IsInStock == isInStock.Value);

            if (!string.IsNullOrEmpty(installationDifficulty))
                query = query.Where(a => a.InstallationDifficulty == installationDifficulty);

            return Task.FromResult(Result<IEnumerable<CarAccessory>>.Success(query));
        }

        public Task<Result<CarAccessory>> CreateAsync(CarAccessory accessory)
        {
            accessory.Id = Guid.NewGuid().ToString();

            if (_accessories.TryAdd(accessory.Id, accessory))
            {
                return Task.FromResult(Result<CarAccessory>.Success(accessory));
            }

            return Task.FromResult(Result<CarAccessory>.Failure(
                new Error("DatabaseError", "Nie udało się dodać akcesorium")));
        }

        public async Task<Result<CarAccessory>> UpdateAsync(string id, CarAccessory accessory)
        {
            var getResult = await GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<CarAccessory>.Failure(getResult.Error);

            accessory.Id = id;
            _accessories[id] = accessory;

            return Result<CarAccessory>.Success(accessory);
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            var getResult = await GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<bool>.Failure(getResult.Error);

            if (_accessories.TryRemove(id, out _))
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                new Error("DatabaseError", $"Nie udało się usunąć akcesorium o ID: {id}"));
        }
    }
}
