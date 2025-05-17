using System;

namespace KonfiguratorSamochodowy.Api.Repositories.Models;

public class Engine
{
    /// <summary>
    /// Unikalny identyfikator silnika
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Identyfikator silnika (używany w API)
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Nazwa silnika
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Typ silnika
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Pojemność silnika w cm³
    /// </summary>
    public int? Capacity { get; set; }

    /// <summary>
    /// Pojemność silnika (string na potrzeby bazy danych)
    /// </summary>
    public string Pojemnosc { get; set; } = string.Empty;

    /// <summary>
    /// Typ silnika (string na potrzeby bazy danych)
    /// </summary>
    public string Typ { get; set; } = string.Empty;

    /// <summary>
    /// Moc silnika w KM
    /// </summary>
    public int Power { get; set; }

    /// <summary>
    /// Moc silnika w KM (short na potrzeby bazy danych)
    /// </summary>
    public short Moc { get; set; }

    /// <summary>
    /// Moment obrotowy w Nm
    /// </summary>
    public int Torque { get; set; }

    /// <summary>
    /// Rodzaj paliwa
    /// </summary>
    public string FuelType { get; set; } = string.Empty;

    /// <summary>
    /// Ilość cylindrów
    /// </summary>
    public int? Cylinders { get; set; }

    /// <summary>
    /// Rodzaj skrzyni biegów
    /// </summary>
    public string Transmission { get; set; } = string.Empty;

    /// <summary>
    /// Ilość biegów
    /// </summary>
    public int Gears { get; set; }

    /// <summary>
    /// Rodzaj napędu
    /// </summary>
    public string DriveType { get; set; } = string.Empty;

    /// <summary>
    /// Zużycie paliwa w l/100km
    /// </summary>
    public decimal FuelConsumption { get; set; }

    /// <summary>
    /// Emisja CO2 w g/km
    /// </summary>
    public int CO2Emission { get; set; }

    /// <summary>
    /// Opis silnika
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Czy silnik jest aktywny
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Data utworzenia
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data aktualizacji
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
