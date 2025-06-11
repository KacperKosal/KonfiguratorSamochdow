using KonfiguratorSamochodowy.Api.Common.Services;
using KonfiguratorSamochodowy.Api.Repositories.Interfaces;

namespace KonfiguratorSamochodowy.Api.Services;

internal class RefreshJwtService(IUserRepository userRepository, IJwtService jwtService) : IRefreshJwtService
{
    public async Task<string> RefreshJwtAsync(string refreshToken, int userId)
    {
        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new ArgumentException("Invalid user ID");
        }

        if (user.RefreshToken != refreshToken || user.RefreshTokenExpires == null || user.RefreshTokenExpires <= DateTime.UtcNow)
        {
            throw new ArgumentException("Invalid refresh token");
        }

        var newToken = jwtService.GenerateToken(userId, user.Rola, user.ImieNazwisko);

        return newToken;
    }
}
