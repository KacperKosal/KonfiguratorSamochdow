namespace KonfiguratorSamochodowy.Api.Requests;

public record ContactRequest(
    string Name,
    string Email,
    string Phone,
    string Subject,
    string Message
);