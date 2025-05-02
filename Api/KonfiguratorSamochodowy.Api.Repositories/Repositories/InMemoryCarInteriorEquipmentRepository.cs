using KonfiguratorSamochodowy.Api.Repositories.Enums;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using System.Collections.Concurrent;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

    public class InMemoryCarInteriorEquipmentRepository : ICarInteriorEquipmentRepository
    {
        private static readonly ConcurrentDictionary<string, CarInteriorEquipment> _equipment = new();
        
        // Inicjalizacja danych dla BMW
        static InMemoryCarInteriorEquipmentRepository()
        {
            // BMW X5
            var bmwX5Id = "1";
            var bmwX5Model = "X5";
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.SeatColor,
                Value = "Black Leather",
                AdditionalPrice = 0,
                Description = "Standardowa czarna skórzana tapicerka",
                IsDefault = true,
                ColorCode = "#000000"
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.SeatColor,
                Value = "Cognac Leather",
                AdditionalPrice = 1500,
                Description = "Luksusowa tapicerka skórzana w kolorze koniaku",
                IsDefault = false,
                ColorCode = "#703200"
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.SeatHeating,
                Value = "Enabled",
                AdditionalPrice = 1200,
                Description = "Podgrzewane fotele przednie i tylne",
                IsDefault = false
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.SeatHeating,
                Value = "Disabled",
                AdditionalPrice = 0,
                Description = "Brak podgrzewania foteli",
                IsDefault = true
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.AdjustableHeadrests,
                Value = "Yes",
                AdditionalPrice = 500,
                Description = "Regulowane zagłówki z pamięcią ustawień",
                IsDefault = false
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.AdjustableHeadrests,
                Value = "No",
                AdditionalPrice = 0,
                Description = "Standardowe zagłówki",
                IsDefault = true
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.MultifunctionSteeringWheel,
                Value = "Yes",
                AdditionalPrice = 800,
                Description = "Multifunkcyjna kierownica z przyciskami sterowania",
                IsDefault = true
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.MultifunctionSteeringWheel,
                Value = "No",
                AdditionalPrice = 0,
                Description = "Standardowa kierownica",
                IsDefault = false
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.RadioType,
                Value = "Standard",
                AdditionalPrice = 0,
                Description = "Standardowy system audio",
                IsDefault = true,
                HasNavigation = false,
                HasPremiumSound = false
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.RadioType,
                Value = "Professional Navigation",
                AdditionalPrice = 2500,
                Description = "System audio z profesjonalną nawigacją",
                IsDefault = false,
                HasNavigation = true,
                HasPremiumSound = false
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.RadioType,
                Value = "Harman Kardon Premium",
                AdditionalPrice = 4500,
                Description = "System audio Harman Kardon z profesjonalną nawigacją",
                IsDefault = false,
                HasNavigation = true,
                HasPremiumSound = true
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.AmbientLighting,
                Value = "Blue",
                AdditionalPrice = 600,
                Description = "Niebieskie oświetlenie ambientowe",
                IsDefault = true,
                ColorCode = "#0000FF",
                Intensity = 5
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.AmbientLighting,
                Value = "White",
                AdditionalPrice = 600,
                Description = "Białe oświetlenie ambientowe",
                IsDefault = false,
                ColorCode = "#FFFFFF",
                Intensity = 5
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.AmbientLighting,
                Value = "RGB",
                AdditionalPrice = 1200,
                Description = "Wielokolorowe oświetlenie ambientowe z wyborem barwy",
                IsDefault = false,
                ColorCode = "#FF0000",
                Intensity = 7
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.CruiseControl,
                Value = "Standard",
                AdditionalPrice = 500,
                Description = "Standardowy tempomat",
                IsDefault = true,
                ControlType = "Standard"
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.CruiseControl,
                Value = "Adaptive",
                AdditionalPrice = 2000,
                Description = "Adaptacyjny tempomat z funkcją stop & go",
                IsDefault = false,
                ControlType = "Adaptive"
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.CruiseControl,
                Value = "None",
                AdditionalPrice = 0,
                Description = "Brak tempomatu",
                IsDefault = false,
                ControlType = "None"
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.ElectricMirrors,
                Value = "Yes",
                AdditionalPrice = 400,
                Description = "Elektrycznie sterowane lusterka boczne",
                IsDefault = true
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwX5Id,
                CarModel = bmwX5Model,
                Type = InteriorEquipmentType.ElectricMirrors,
                Value = "No",
                AdditionalPrice = 0,
                Description = "Manualnie sterowane lusterka boczne",
                IsDefault = false
            });
            
            // BMW 3 Series
            var bmw3Id = "2";
            var bmw3Model = "3 Series";
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmw3Id,
                CarModel = bmw3Model,
                Type = InteriorEquipmentType.SeatColor,
                Value = "Black Fabric",
                AdditionalPrice = 0,
                Description = "Standardowa czarna tapicerka materiałowa",
                IsDefault = true,
                ColorCode = "#000000"
            });
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmw3Id,
                CarModel = bmw3Model,
                Type = InteriorEquipmentType.SeatColor,
                Value = "Beige Leather",
                AdditionalPrice = 2000,
                Description = "Beżowa tapicerka skórzana",
                IsDefault = false,
                ColorCode = "#F5F5DC"
            });
            
            // Dodaj więcej wyposażenia dla BMW 3 Series...
            
            // BMW i4
            var bmwI4Id = "3";
            var bmwI4Model = "i4";
            
            AddEquipment(new CarInteriorEquipment
            {
                Id = Guid.NewGuid().ToString(),
                CarId = bmwI4Id,
                CarModel = bmwI4Model,
                Type = InteriorEquipmentType.SeatColor,
                Value = "Blue Fabric",
                AdditionalPrice = 0,
                Description = "Tapicerka materiałowa z niebieskimi akcentami",
                IsDefault = true,
                ColorCode = "#2F4F8F"
            });
            
            // Dodaj więcej wyposażenia dla BMW i4...
        }
        
        private static void AddEquipment(CarInteriorEquipment equipment)
        {
            _equipment[equipment.Id] = equipment;
        }
        
        public Task<Result<IEnumerable<CarInteriorEquipment>>> GetAllAsync()
        {
            return Task.FromResult(Result<IEnumerable<CarInteriorEquipment>>.Success(_equipment.Values));
        }

        public Task<Result<CarInteriorEquipment>> GetByIdAsync(string id)
        {
            if (_equipment.TryGetValue(id, out var equipment))
            {
                return Task.FromResult(Result<CarInteriorEquipment>.Success(equipment));
            }

            return Task.FromResult(Result<CarInteriorEquipment>.Failure(
                new Error("NotFound", $"Nie znaleziono elementu wyposażenia o ID: {id}")));
        }

        public Task<Result<IEnumerable<CarInteriorEquipment>>> GetByCarIdAsync(string carId)
        {
            var result = _equipment.Values.Where(e => e.CarId == carId);
            return Task.FromResult(Result<IEnumerable<CarInteriorEquipment>>.Success(result));
        }

        public Task<Result<IEnumerable<CarInteriorEquipment>>> GetByCarModelAsync(string carModel)
        {
            var result = _equipment.Values.Where(e => e.CarModel == carModel);
            return Task.FromResult(Result<IEnumerable<CarInteriorEquipment>>.Success(result));
        }

        public Task<Result<IEnumerable<CarInteriorEquipment>>> GetByTypeAsync(string type)
        {
            var result = _equipment.Values.Where(e => e.Type == type);
            return Task.FromResult(Result<IEnumerable<CarInteriorEquipment>>.Success(result));
        }

        public Task<Result<IEnumerable<CarInteriorEquipment>>> GetFilteredAsync(
            string? carId = null, string? carModel = null, string? type = null, bool? isDefault = null, decimal? maxPrice = null)
        {
            var query = _equipment.Values.AsEnumerable();

            if (!string.IsNullOrEmpty(carId))
                query = query.Where(e => e.CarId == carId);

            if (!string.IsNullOrEmpty(carModel))
                query = query.Where(e => e.CarModel == carModel);

            if (!string.IsNullOrEmpty(type))
                query = query.Where(e => e.Type == type);

            if (isDefault.HasValue)
                query = query.Where(e => e.IsDefault == isDefault.Value);

            if (maxPrice.HasValue)
                query = query.Where(e => e.AdditionalPrice <= maxPrice.Value);

            return Task.FromResult(Result<IEnumerable<CarInteriorEquipment>>.Success(query));
        }

        public Task<Result<CarInteriorEquipment>> CreateAsync(CarInteriorEquipment equipment)
        {
            equipment.Id = Guid.NewGuid().ToString();
            
            if (_equipment.TryAdd(equipment.Id, equipment))
            {
                return Task.FromResult(Result<CarInteriorEquipment>.Success(equipment));
            }

            return Task.FromResult(Result<CarInteriorEquipment>.Failure(
                new Error("DatabaseError", "Nie udało się dodać elementu wyposażenia")));
        }

        public async Task<Result<CarInteriorEquipment>> UpdateAsync(string id, CarInteriorEquipment equipment)
        {
            var getResult = await GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<CarInteriorEquipment>.Failure(getResult.Error);

            equipment.Id = id;
            
            _equipment[id] = equipment;
            
            return Result<CarInteriorEquipment>.Success(equipment);
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            var getResult = await GetByIdAsync(id);
            if (!getResult.IsSuccess)
                return Result<bool>.Failure(getResult.Error);

            if (_equipment.TryRemove(id, out _))
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                new Error("DatabaseError", $"Nie udało się usunąć elementu wyposażenia o ID: {id}"));
        }
    }
