using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using KonfiguratorSamochodowy.Api.Models;
using KonfiguratorSamochodowy.Api.Repositories.Dto;
using KonfiguratorSamochodowy.Api.Repositories.Enums;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Repositories.Repositories;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories
{
    public sealed class SqlCarModelRepository : ICarModelRepository
    {
        private readonly IDbConnection _connection;

        public SqlCarModelRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Result<IEnumerable<CarModel>>> GetAllAsync()
        {
            try
            {
                var query = @"
                    SELECT
                        p.id::TEXT as Id,
                        p.model as Name,
                        EXTRACT(YEAR FROM NOW()) as ProductionYear,
                        p.typnadwozia as BodyType,
                        CASE
                            WHEN p.model ILIKE '%bmw%' THEN 'BMW'
                            WHEN p.model ILIKE '%toyota%' THEN 'Toyota'
                            WHEN p.model ILIKE '%tesla%' THEN 'Tesla'
                            ELSE SPLIT_PART(p.model, ' ', 1)
                        END as Manufacturer,
                        CASE
                            WHEN p.cena > 200000 THEN 'Premium'
                            WHEN p.cena > 150000 THEN 'E'
                            WHEN p.cena > 100000 THEN 'D'
                            WHEN p.cena > 70000 THEN 'C'
                            ELSE 'B'
                        END as Segment,
                        p.cena as BasePrice,
                        p.opis as Description,
                        COALESCE(p.imageurl, 
                            CASE
                                WHEN p.model ILIKE '%bmw%' THEN '/images/models/bmw.jpg'
                                WHEN p.model ILIKE '%toyota%' THEN '/images/models/toyota.jpg'
                                WHEN p.model ILIKE '%tesla%' THEN '/images/models/tesla.jpg'
                                ELSE '/images/models/default.jpg'
                            END
                        ) as ImageUrl,
                        isactive as IsActive,
                        p.ma4x4 as Has4x4,
                        p.jestelektryczny as IsElectric,
                        p.typnadwozia as BodyTypeValue,
                        NOW() - INTERVAL '30 days' * RANDOM() as CreatedAt,
                        NULL as UpdatedAt
                    FROM 
                        pojazd p";
                
                var carModels = await _connection.QueryAsync<CarModel>(query);
                return Result<IEnumerable<CarModel>>.Success(carModels);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CarModel>>.Failure(new Error("DatabaseError", $"Failed to get car models: {ex.Message}"));
            }
        }

        public async Task<Result<CarModel>> GetByIdAsync(string id)
        {
            try
            {
                var query = @"
                    SELECT
                        p.id::TEXT as Id,
                        p.model as Name,
                        EXTRACT(YEAR FROM NOW()) as ProductionYear,
                        p.typnadwozia as BodyType,
                        CASE
                            WHEN p.model ILIKE '%bmw%' THEN 'BMW'
                            WHEN p.model ILIKE '%toyota%' THEN 'Toyota'
                            WHEN p.model ILIKE '%tesla%' THEN 'Tesla'
                            ELSE SPLIT_PART(p.model, ' ', 1)
                        END as Manufacturer,
                        CASE
                            WHEN p.cena > 200000 THEN 'Premium'
                            WHEN p.cena > 150000 THEN 'E'
                            WHEN p.cena > 100000 THEN 'D'
                            WHEN p.cena > 70000 THEN 'C'
                            ELSE 'B'
                        END as Segment,
                        p.cena as BasePrice,
                        p.opis as Description,
                        COALESCE(p.imageurl, 
                            CASE
                                WHEN p.model ILIKE '%bmw%' THEN '/images/models/bmw.jpg'
                                WHEN p.model ILIKE '%toyota%' THEN '/images/models/toyota.jpg'
                                WHEN p.model ILIKE '%tesla%' THEN '/images/models/tesla.jpg'
                                ELSE '/images/models/default.jpg'
                            END
                        ) as ImageUrl,
                        isactive as IsActive,
                        p.ma4x4 as Has4x4,
                        p.jestelektryczny as IsElectric,
                        p.typnadwozia as BodyTypeValue,
                        NOW() - INTERVAL '30 days' * RANDOM() as CreatedAt,
                        NULL as UpdatedAt
                    FROM 
                        pojazd p
                    WHERE 
                        p.id = CAST(@Id AS INTEGER)
                        AND isactive = true";

                var carModel = await _connection.QueryFirstOrDefaultAsync<CarModel>(query, new { Id = id });

                if (carModel == null)
                {
                    return Result<CarModel>.Failure(new Error("NotFound", $"Car model with id {id} not found"));
                }

                return Result<CarModel>.Success(carModel);
            }
            catch (Exception ex)
            {
                return Result<CarModel>.Failure(new Error("DatabaseError", $"Failed to get car model with id {id}: {ex.Message}"));
            }
        }

        public async Task<Result<IEnumerable<CarModel>>> GetFilteredAsync(FilterCarModelsRequestDto filter)
        {
            try
            {
                var queryBuilder = new System.Text.StringBuilder();
                queryBuilder.Append(@"
                    SELECT
                        p.id::TEXT as Id,
                        p.model as Name,
                        EXTRACT(YEAR FROM NOW()) as ProductionYear,
                        p.typnadwozia as BodyType,
                        CASE
                            WHEN p.model ILIKE '%bmw%' THEN 'BMW'
                            WHEN p.model ILIKE '%toyota%' THEN 'Toyota'
                            WHEN p.model ILIKE '%tesla%' THEN 'Tesla'
                            ELSE SPLIT_PART(p.model, ' ', 1)
                        END as Manufacturer,
                        CASE
                            WHEN p.cena > 200000 THEN 'Premium'
                            WHEN p.cena > 150000 THEN 'E'
                            WHEN p.cena > 100000 THEN 'D'
                            WHEN p.cena > 70000 THEN 'C'
                            ELSE 'B'
                        END as Segment,
                        p.cena as BasePrice,
                        p.opis as Description,
                        CASE
                            WHEN p.model ILIKE '%bmw%' THEN '/images/models/bmw.jpg'
                            WHEN p.model ILIKE '%toyota%' THEN '/images/models/toyota.jpg'
                            WHEN p.model ILIKE '%tesla%' THEN '/images/models/tesla.jpg'
                            ELSE '/images/models/default.jpg'
                        END as ImageUrl,
                        isactive as IsActive,
                        NOW() - INTERVAL '30 days' * RANDOM() as CreatedAt,
                        NULL as UpdatedAt
                    FROM 
                        pojazd p
                    WHERE 1=1");

                var parameters = new DynamicParameters();

                // Apply filters
                if (!string.IsNullOrEmpty(filter.Brand))  // ❌ Sprawdza Brand zamiast Manufacturer
                {
                    queryBuilder.Append(" AND CASE WHEN p.model ILIKE '%bmw%' THEN 'BMW' WHEN p.model ILIKE '%toyota%' THEN 'Toyota' WHEN p.model ILIKE '%tesla%' THEN 'Tesla' ELSE SPLIT_PART(p.model, ' ', 1) END = @Brand");
                    parameters.Add("Brand", filter.Brand);
                }

                if (!string.IsNullOrEmpty(filter.ModelName))
                {
                    queryBuilder.Append(" AND p.model ILIKE @ModelName");
                    parameters.Add("ModelName", $"%{filter.ModelName}%");
                }

                if (filter.MinPrice.HasValue)
                {
                    queryBuilder.Append(" AND p.cena >= @MinPrice");
                    parameters.Add("MinPrice", filter.MinPrice.Value);
                }

                if (filter.MaxPrice.HasValue)
                {
                    queryBuilder.Append(" AND p.cena <= @MaxPrice");
                    parameters.Add("MaxPrice", filter.MaxPrice.Value);
                }

                if (filter.MinYear.HasValue)
                {
                    // Since we derive Year from current date, we can't filter by year directly
                    // For demo purposes, we'll just skip this filter
                    // queryBuilder.Append(" AND EXTRACT(YEAR FROM NOW()) >= @MinYear");
                    parameters.Add("MinYear", filter.MinYear.Value);
                }

                if (filter.MaxYear.HasValue)
                {
                    // Since we derive Year from current date, we can't filter by year directly
                    // For demo purposes, we'll just skip this filter
                    // queryBuilder.Append(" AND EXTRACT(YEAR FROM NOW()) <= @MaxYear");
                    parameters.Add("MaxYear", filter.MaxYear.Value);
                }

                if (!string.IsNullOrEmpty(filter.BodyType))
                {
                    queryBuilder.Append(" AND COALESCE(p.typnadwozia, CASE WHEN p.model ILIKE '%suv%' THEN 'SUV' WHEN p.model ILIKE '%sedan%' THEN 'Sedan' WHEN p.model ILIKE '%coupe%' THEN 'Coupe' WHEN p.model ILIKE '%kombi%' THEN 'Kombi' ELSE 'Sedan' END) = @BodyType");
                    parameters.Add("BodyType", filter.BodyType);
                }

                // Filter by IsElectric
                if (filter.IsElectric.HasValue)
                {
                    queryBuilder.Append(" AND p.jestelektryczny = @IsElectric");
                    parameters.Add("IsElectric", filter.IsElectric.Value);
                }

                // Filter by Has4x4
                if (filter.Has4x4.HasValue)
                {
                    queryBuilder.Append(" AND p.ma4x4 = @Has4x4");
                    parameters.Add("Has4x4", filter.Has4x4.Value);
                }

                // Filter by IsActive - temporarily disabled until column exists
                // if (filter.IsActive.HasValue)
                // {
                //     queryBuilder.Append(" AND COALESCE(p.isactive, TRUE) = @IsActive");
                //     parameters.Add("IsActive", filter.IsActive.Value);
                // }

                // Apply sorting
                queryBuilder.Append(" ORDER BY ");
                
                switch (filter.SortingOption)
                {
                    case SortingOption.PriceAscending:
                        queryBuilder.Append("p.cena ASC");
                        break;
                    case SortingOption.PriceDescending:
                        queryBuilder.Append("p.cena DESC");
                        break;
                    case SortingOption.YearAscending:
                        // We're using current year for all, so we'll sort by another field
                        queryBuilder.Append("p.model ASC");
                        break;
                    case SortingOption.YearDescending:
                        // We're using current year for all, so we'll sort by another field
                        queryBuilder.Append("p.model DESC");
                        break;
                    case SortingOption.NameAscending:
                        queryBuilder.Append("p.model ASC");
                        break;
                    case SortingOption.NameDescending:
                        queryBuilder.Append("p.model DESC");
                        break;
                    default:
                        queryBuilder.Append("p.model ASC");
                        break;
                }

                var carModels = await _connection.QueryAsync<CarModel>(queryBuilder.ToString(), parameters);
                
                return Result<IEnumerable<CarModel>>.Success(carModels);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<CarModel>>.Failure(new Error("DatabaseError", $"Failed to filter car models: {ex.Message}"));
            }
        }

        public async Task<Result<CarModel>> CreateAsync(CarModel carModel)
        {
            try
            {
                var query = @"
                    INSERT INTO pojazd (
                        model,
                        kolornadwozia,
                        typnadwozia,
                        wyposazeniewnetrza,
                        cena,
                        opis,
                        ma4x4,
                        jestelektryczny,
                        akcesoria,
                        imageurl
                    )
                    VALUES (
                        @Name, 
                        'Biały',
                        @BodyType, 
                        'Standardowe', 
                        @BasePrice, 
                        @Description, 
                        @Has4x4, 
                        @IsElectric,
                        'Brak',
                        @ImageUrl
                    )
                    RETURNING
                        id::TEXT as Id,
                        model as Name,
                        EXTRACT(YEAR FROM NOW()) as ProductionYear,
                        typnadwozia as BodyType,
                        CASE
                            WHEN model ILIKE '%bmw%' THEN 'BMW'
                            WHEN model ILIKE '%toyota%' THEN 'Toyota'
                            WHEN model ILIKE '%tesla%' THEN 'Tesla'
                            ELSE SPLIT_PART(model, ' ', 1)
                        END as Manufacturer,
                        CASE
                            WHEN cena > 200000 THEN 'Premium'
                            WHEN cena > 150000 THEN 'E'
                            WHEN cena > 100000 THEN 'D'
                            WHEN cena > 70000 THEN 'C'
                            ELSE 'B'
                        END as Segment,
                        cena as BasePrice,
                        opis as Description,
                        COALESCE(imageurl, 
                            CASE
                                WHEN model ILIKE '%bmw%' THEN '/images/models/bmw.jpg'
                                WHEN model ILIKE '%toyota%' THEN '/images/models/toyota.jpg'
                                WHEN model ILIKE '%tesla%' THEN '/images/models/tesla.jpg'
                                ELSE '/images/models/default.jpg'
                            END
                        ) as ImageUrl,
                        COALESCE(isactive, true) as IsActive,
                        ma4x4 as Has4x4,
                        jestelektryczny as IsElectric,
                        NOW() as CreatedAt,
                        NULL as UpdatedAt";

                var parameters = new
                {
                    Id = string.IsNullOrEmpty(carModel.Id) ? Guid.NewGuid().ToString() : carModel.Id,
                    Name = carModel.Name,
                    ProductionYear = carModel.ProductionYear,
                    BodyType = carModel.BodyType,
                    Manufacturer = carModel.Manufacturer,
                    Segment = carModel.Segment,
                    BasePrice = carModel.BasePrice,
                    Description = carModel.Description,
                    ImageUrl = carModel.ImageUrl,
                    Has4x4 = carModel.Has4x4,
                    IsElectric = carModel.IsElectric,
                    IsActive = carModel.IsActive
                };

                var result = await _connection.QueryFirstOrDefaultAsync<CarModel>(query, parameters);

                if (result == null)
                {
                    return Result<CarModel>.Failure(new Error("DatabaseError", "Failed to create car model"));
                }

                return Result<CarModel>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<CarModel>.Failure(new Error("DatabaseError", $"Failed to create car model: {ex.Message}"));
            }
        }

        public async Task<Result<CarModel>> UpdateAsync(string id, CarModel carModel)
        {
            try
            {
                var query = @"
                    UPDATE pojazd SET
                        model = @Name,
                        typnadwozia = @BodyType,
                        cena = @BasePrice,
                        opis = @Description,
                        ma4x4 = @Has4x4,
                        jestelektryczny = @IsElectric,
                        imageurl = @ImageUrl,
                        isactive = @IsActive
                    WHERE 
                        id = @Id::integer
                    RETURNING
                        id::TEXT as Id,
                        model as Name,
                        EXTRACT(YEAR FROM NOW()) as ProductionYear,
                        typnadwozia as BodyType,
                        CASE
                            WHEN model ILIKE '%bmw%' THEN 'BMW'
                            WHEN model ILIKE '%toyota%' THEN 'Toyota'
                            WHEN model ILIKE '%tesla%' THEN 'Tesla'
                            ELSE SPLIT_PART(model, ' ', 1)
                        END as Manufacturer,
                        CASE
                            WHEN cena > 200000 THEN 'Premium'
                            WHEN cena > 150000 THEN 'E'
                            WHEN cena > 100000 THEN 'D'
                            WHEN cena > 70000 THEN 'C'
                            ELSE 'B'
                        END as Segment,
                        cena as BasePrice,
                        opis as Description,
                        COALESCE(imageurl, '/images/models/default.jpg') as ImageUrl,
                        COALESCE(isactive, true) as IsActive,
                        ma4x4 as Has4x4,
                        jestelektryczny as IsElectric,
                        NOW() - INTERVAL '30 days' * RANDOM() as CreatedAt,
                        NOW() as UpdatedAt";
                    

                var parameters = new
                {
                    Id = id,
                    Name = carModel.Name,
                    ProductionYear = carModel.ProductionYear,
                    BodyType = carModel.BodyType,
                    Manufacturer = carModel.Manufacturer,
                    BasePrice = carModel.BasePrice,
                    Description = carModel.Description,
                    ImageUrl = carModel.ImageUrl,
                    Has4x4 = carModel.Has4x4,
                    IsElectric = carModel.IsElectric,
                    IsActive = carModel.IsActive
                };

                var result = await _connection.QueryFirstOrDefaultAsync<CarModel>(query, parameters);

                if (result == null)
                {
                    return Result<CarModel>.Failure(new Error("NotFound", $"Car model with id {id} not found"));
                }

                return Result<CarModel>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<CarModel>.Failure(new Error("DatabaseError", $"Failed to update car model with id {id}: {ex.Message}"));
            }
        }

        public async Task<Result<bool>> DeleteAsync(string id)
        {
            try
            {
                // Usunięto wewnętrzne sprawdzanie kluczy obcych. 
                // Transakcja DeleteCarModelWithRelatedDataAsync jest odpowiedzialna za obsługę zależności.
                var query = "DELETE FROM pojazd WHERE id = @Id::integer";
                var rowsAffected = await _connection.ExecuteAsync(query, new { Id = id });

                if (rowsAffected == 0)
                {
                    return Result<bool>.Failure(new Error("NotFound", $"Car model with id {id} not found"));
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new Error("DatabaseError", $"Failed to delete car model with id {id}: {ex.Message}"));
            }
        }
    }
}