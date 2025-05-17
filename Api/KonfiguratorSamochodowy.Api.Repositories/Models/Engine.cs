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
    public string Id { get; set; }

    /// <summary>
    /// Nazwa silnika
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Typ silnika
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Pojemność silnika w cm³
    /// </summary>
    public int? Capacity { get; set; }

    /// <summary>
    /// Pojemność silnika (string na potrzeby bazy danych)
    /// </summary>
    public string Pojemnosc { get; set; }

    /// <summary>
    /// Typ silnika (string na potrzeby bazy danych)
    /// </summary>
    public string Typ { get; set; }

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
    public string FuelType { get; set; }

    /// <summary>
    /// Ilość cylindrów
    /// </summary>
    public int? Cylinders { get; set; }

    /// <summary>
    /// Rodzaj skrzyni biegów
    /// </summary>
    public string Transmission { get; set; }

    /// <summary>
    /// Ilość biegów
    /// </summary>
    public int Gears { get; set; }

    /// <summary>
    /// Rodzaj napędu
    /// </summary>
    public string DriveType { get; set; }

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
    public string Description { get; set; }

    /// <summary>
    /// Czy silnik jest aktywny
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Data utworzenia
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data aktualizacji
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
