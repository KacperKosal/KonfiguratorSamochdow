using Dapper;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;
using KonfiguratorSamochodowy.Api.Repositories.Models;
using System.Data;

namespace KonfiguratorSamochodowy.Api.Repositories.Repositories;

/// <summary>
/// Implementacja repozytorium użytkowników
/// </summary>
internal sealed class UserRepository : IUserRepository
{
    private readonly IDbConnection _connection;

    /// <summary>
    /// Inicjalizuje nową instancję klasy UserRepository
    /// </summary>
    /// <param name="connection">Połączenie do bazy danych</param>
    public UserRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Pobiera użytkownika na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator użytkownika</param>
    /// <returns>Obiekt użytkownika lub null, jeśli nie znaleziono</returns>
    public async Task<User?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM Uzytkownik WHERE ID = @Id";
        return await _connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    /// <summary>
    /// Pobiera użytkownika na podstawie adresu email
    /// </summary>
    /// <param name="email">Adres email użytkownika</param>
    /// <returns>Obiekt użytkownika lub null, jeśli nie znaleziono</returns>
    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = "SELECT * FROM Uzytkownik WHERE Email = @Email";
        return await _connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    /// <summary>
    /// Tworzy nowego użytkownika
    /// </summary>
    /// <param name="user">Obiekt użytkownika do utworzenia</param>
    /// <returns>Identyfikator utworzonego użytkownika</returns>
    public async Task<int> CreateAsync(User user)
    {
        const string sql = @"
            INSERT INTO Uzytkownik (ImieNazwisko, Email, Haslo, Rola)
            VALUES (@ImieNazwisko, @Email, @Haslo, @Rola)
            RETURNING ID";
        
        return await _connection.ExecuteScalarAsync<int>(sql, user);
    }

    /// <summary>
    /// Aktualizuje dane użytkownika
    /// </summary>
    /// <param name="user">Obiekt użytkownika z zaktualizowanymi danymi</param>
    public async Task UpdateAsync(User user)
    {
        const string sql = @"
            UPDATE Uzytkownik 
            SET ImieNazwisko = @ImieNazwisko,
                Email = @Email,
                Haslo = @Haslo,
                Rola = @Rola,
            WHERE ID = @ID";
        
        await _connection.ExecuteAsync(sql, user);
    }

    /// <summary>
    /// Usuwa użytkownika na podstawie identyfikatora
    /// </summary>
    /// <param name="id">Identyfikator użytkownika do usunięcia</param>
    public async Task DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Uzytkownik WHERE ID = @Id";
        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Sprawdza, czy użytkownik o podanym identyfikatorze istnieje
    /// </summary>
    /// <param name="id">Identyfikator użytkownika</param>
    /// <returns>True, jeśli użytkownik istnieje; w przeciwnym razie false</returns>
    public async Task<bool> ExistsAsync(int id)
    {
        const string sql = "SELECT COUNT(1) FROM Uzytkownik WHERE ID = @Id";
        return await _connection.ExecuteScalarAsync<bool>(sql, new { Id = id });
    }
} 