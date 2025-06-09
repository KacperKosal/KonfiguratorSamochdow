using KonfiguratorSamochodowy.Api.Requests;
using KonfiguratorSamochodowy.Api.Services;

namespace KonfiguratorSamochodowy.Api.Endpoints;

public static class ContactEndpoint
{
    public static void MapEndPoint(WebApplication app)
    {
        app.MapPost("/api/contact", async (ContactRequest request, IEmailService emailService) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Message))
            {
                return Results.BadRequest(new { error = "Nazwa, email i wiadomość są wymagane." });
            }

            if (!IsValidEmail(request.Email))
            {
                return Results.BadRequest(new { error = "Nieprawidłowy format adresu email." });
            }

            var success = await emailService.SendContactEmailAsync(request);
            
            if (success)
            {
                return Results.Ok(new { message = "Wiadomość została wysłana pomyślnie." });
            }
            else
            {
                return Results.Problem("Wystąpił błąd podczas wysyłania wiadomości. Spróbuj ponownie później.");
            }
        })
        .WithTags("Contact")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Wysyła wiadomość kontaktową",
            Description = "Endpoint do wysyłania wiadomości kontaktowych na adres konfiguratorsamochodwy@gmail.com"
        });
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}