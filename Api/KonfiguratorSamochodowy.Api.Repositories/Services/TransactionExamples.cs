using System;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using KonfiguratorSamochodowy.Api.Repositories.Repositories;
using Npgsql;
using System.Text.Json;
using System.Text.Json.Serialization;
using KonfiguratorSamochodowy.Api.Repositories.Helpers;

namespace KonfiguratorSamochodowy.Api.Repositories.Services
{
    /// <summary>
    /// Przykłady implementacji transakcji dla różnych operacji w systemie
    /// </summary>
    public class TransactionExamples
    {
        private readonly ITransactionService _transactionService;
        private readonly string _connectionString;
        private readonly JsonSerializerOptions _jsonOptions;

        public TransactionExamples(ITransactionService transactionService, string connectionString)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
        }

        /// <summary>
        /// Transakcja 1: Aktualizacja konfiguracji użytkownika wraz z zapisem w historii
        /// </summary>
        public async Task<Result<bool>> UpdateUserConfigurationWithHistoryAsync(
            IUserConfigurationRepository userConfigRepo,
            UserConfiguration newConfiguration,
            UserConfiguration oldConfiguration,
            string changeDescription)
        {
            try
            {
                return await _transactionService.ExecuteInTransactionAsync(async () =>
                {
                    using var connection = new NpgsqlConnection(_connectionString);
                    await connection.OpenAsync();

                    // 1. Aktualizacja konfiguracji
                    var updateResult = await userConfigRepo.UpdateUserConfigurationAsync(newConfiguration);
                    if (!updateResult.IsSuccess)
                    {
                        throw new Exception($"Failed to update configuration: {updateResult.Error?.Message}");
                    }

                    // 2. Zapis w historii zmian
                    const string historySql = @"
                        INSERT INTO configuration_history 
                        (configuration_id, user_id, change_description, old_values, new_values, created_at)
                        VALUES 
                        (@ConfigurationId, @UserId, @Description, @OldValues, @NewValues, @CreatedAt)";

                    var historyParameters = new
                    {
                        ConfigurationId = newConfiguration.Id,
                        UserId = newConfiguration.UserId,
                        Description = changeDescription,
                        OldValues = oldConfiguration != null ? JsonSerializer.Serialize(oldConfiguration, _jsonOptions) : null,
                        NewValues = JsonSerializer.Serialize(newConfiguration, _jsonOptions),
                        CreatedAt = DateTime.UtcNow
                    };

                    await connection.ExecuteAsync(historySql, historyParameters);

                    return Result<bool>.Success(true);
                });
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new Error("TRANSACTION_ERROR", $"Error in UpdateUserConfigurationWithHistory: {ex.Message}"));
            }
        }

        /// <summary>
        /// Transakcja 2: Usunięcie modelu samochodu wraz z powiązanymi danymi
        /// </summary>
        public async Task<Result<bool>> DeleteCarModelWithRelatedDataAsync(
            ICarModelRepository carModelRepo,
            ICarModelEngineRepository carModelEngineRepo,
            IEngineRepository engineRepo,
            ICarModelImageRepository carModelImageRepo,
            string carModelId)
        {
            try
            {
                return await _transactionService.ExecuteInTransactionAsync(async () =>
                {
                    // 1. Sprawdzenie czy model istnieje i czy można go usunąć
                    var carModel = await carModelRepo.GetByIdAsync(carModelId);
                    if (!carModel.IsSuccess)
                    {
                        throw new Exception($"Car model not found: {carModel.Error?.Message}");
                    }

                    // 2. Usunięcie powiązań silników z modelem (modelsilnik)
                    var enginesAssociations = await carModelEngineRepo.GetByCarModelIdAsync(carModelId);
                    if (enginesAssociations.IsSuccess)
                    {
                        foreach (var association in enginesAssociations.Value)
                        {
                            var deleteAssociationResult = await carModelEngineRepo.DeleteAsync(association.Id);
                            if (!deleteAssociationResult.IsSuccess)
                            {
                                throw new Exception($"Failed to delete engine association: {deleteAssociationResult.Error?.Message}");
                            }
                        }
                    }

                    // 3. Usunięcie samych silników powiązanych z modelem (z tabeli Silnik)
                    var deleteEnginesResult = await engineRepo.DeleteByVehicleIdAsync(carModelId);
                    if (!deleteEnginesResult.IsSuccess)
                    {
                        throw new Exception($"Failed to delete associated engines from Silnik table: {deleteEnginesResult.Error?.Message}");
                    }

                    // 4. Usunięcie zdjęć powiązanych z modelem (z tabeli pojazd_zdjecie)
                    var deleteImagesResult = await carModelImageRepo.DeleteImagesByCarModelIdAsync(carModelId);
                    if (!deleteImagesResult.IsSuccess)
                    {
                        throw new Exception($"Failed to delete associated images from pojazd_zdjecie table: {deleteImagesResult.Error?.Message}");
                    }

                    // 5. Usunięcie modelu samochodu
                    var deleteResult = await carModelRepo.DeleteAsync(carModelId);
                    if (!deleteResult.IsSuccess)
                    {
                        throw new Exception($"Failed to delete car model: {deleteResult.Error?.Message}");
                    }

                    return Result<bool>.Success(true);
                });
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new Error("TRANSACTION_ERROR", $"Error in DeleteCarModelWithRelatedData: {ex.Message}"));
            }
        }

        /// <summary>
        /// Transakcja 3: Aktualizacja ceny modelu samochodu wraz z przeliczeniem cen konfiguracji
        /// </summary>
        public async Task<Result<bool>> UpdateCarModelPriceWithConfigurationsAsync(
            ICarModelRepository carModelRepo,
            string carModelId,
            decimal newBasePrice)
        {
            try
            {
                return await _transactionService.ExecuteInTransactionAsync(async () =>
                {
                    using var connection = new NpgsqlConnection(_connectionString);
                    await connection.OpenAsync();

                    // 1. Pobranie aktualnego modelu
                    var carModel = await carModelRepo.GetByIdAsync(carModelId);
                    if (!carModel.IsSuccess)
                    {
                        throw new Exception($"Car model not found: {carModel.Error?.Message}");
                    }

                    var oldBasePrice = carModel.Value.BasePrice;

                    // 2. Aktualizacja ceny bazowej
                    var updatedModel = carModel.Value;
                    updatedModel.BasePrice = newBasePrice;
                    var updateResult = await carModelRepo.UpdateAsync(carModelId, updatedModel);
                    if (!updateResult.IsSuccess)
                    {
                        throw new Exception($"Failed to update car model: {updateResult.Error?.Message}");
                    }

                    // 3. Aktualizacja cen wszystkich konfiguracji dla tego modelu
                    const string updateConfigsSql = @"
                        UPDATE user_configurations 
                        SET total_price = total_price + (@NewBasePrice - @OldBasePrice),
                            updated_at = @UpdatedAt
                        WHERE car_model_id = @CarModelId AND is_active = true";

                    var updateParameters = new
                    {
                        NewBasePrice = newBasePrice,
                        OldBasePrice = oldBasePrice,
                        CarModelId = carModelId,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await connection.ExecuteAsync(updateConfigsSql, updateParameters);

                    return Result<bool>.Success(true);
                });
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new Error("TRANSACTION_ERROR", $"Error in UpdateCarModelPriceWithConfigurations: {ex.Message}"));
            }
        }
    }
}