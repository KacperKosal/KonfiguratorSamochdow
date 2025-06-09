using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Services;

public interface IEmailService
{
    Task<bool> SendContactEmailAsync(ContactRequest contactRequest);
}