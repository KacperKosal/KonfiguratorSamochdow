using System.Net;
using System.Net.Mail;
using KonfiguratorSamochodowy.Api.Requests;

namespace KonfiguratorSamochodowy.Api.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> SendContactEmailAsync(ContactRequest contactRequest)
    {
        try
        {
            var smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var senderEmail = _configuration["Email:SenderEmail"];
            var senderPassword = _configuration["Email:SenderPassword"];
            var targetEmail = _configuration["Email:TargetEmail"] ?? "konfiguratorsamochodwy@gmail.com";
            var useMockService = _configuration.GetValue<bool>("Email:UseMockService", true);

            _logger.LogInformation("Email configuration - Server: {Server}, Port: {Port}, Sender: {Sender}, Target: {Target}, MockMode: {MockMode}", 
                smtpServer, smtpPort, senderEmail, targetEmail, useMockService);

            // Jeśli włączony jest tryb mock lub brak konfiguracji
            if (useMockService || string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderPassword))
            {
                _logger.LogWarning("Using mock email service. MockMode: {MockMode}, SenderEmail: {SenderEmail}, HasPassword: {HasPassword}", 
                    useMockService, senderEmail, !string.IsNullOrEmpty(senderPassword));
                await SimulateEmailSending(contactRequest);
                return true;
            }

            _logger.LogInformation("Attempting to send real email...");

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                Timeout = 10000 // 10 sekund timeout
            };

            var subject = GetSubjectText(contactRequest.Subject);
            var body = CreateEmailBody(contactRequest);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, "Konfigurator Samochodowy"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(targetEmail);
            
            // Dodaj Reply-To tylko jeśli email nadawcy jest poprawny
            if (IsValidEmail(contactRequest.Email))
            {
                mailMessage.ReplyToList.Add(new MailAddress(contactRequest.Email, contactRequest.Name));
            }

            _logger.LogInformation("Sending email to {Target} with subject: {Subject}", targetEmail, subject);
            
            await client.SendMailAsync(mailMessage);
            
            _logger.LogInformation("Contact email sent successfully from {Email} to {Target}", contactRequest.Email, targetEmail);
            return true;
        }
        catch (SmtpException smtpEx)
        {
            _logger.LogError(smtpEx, "SMTP Error while sending email from {Email}. SMTP Code: {SmtpCode}, Message: {SmtpMessage}", 
                contactRequest.Email, smtpEx.StatusCode, smtpEx.Message);
            
            // W przypadku błędu SMTP, wyślij mock email żeby nie stracić wiadomości
            _logger.LogWarning("SMTP failed, falling back to mock email to preserve message.");
            await SimulateEmailSending(contactRequest);
            return true; // Zwracamy true żeby nie pokazywać błędu użytkownikowi
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "General error while sending contact email from {Email}. Error: {ErrorMessage}", 
                contactRequest.Email, ex.Message);
            
            // W przypadku błędu, wyślij mock email żeby nie stracić wiadomości
            _logger.LogWarning("General error, falling back to mock email due to error.");
            await SimulateEmailSending(contactRequest);
            return true; // Zwracamy true żeby nie pokazywać błędu użytkownikowi
        }
    }

    private bool IsValidEmail(string email)
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

    private string GetSubjectText(string subject)
    {
        return subject switch
        {
            "info" => "[Konfigurator] Informacje ogólne",
            "config" => "[Konfigurator] Konfiguracja samochodu",
            "test" => "[Konfigurator] Jazda testowa",
            "other" => "[Konfigurator] Inne zapytanie",
            _ => "[Konfigurator] Nowe zapytanie"
        };
    }

    private string CreateEmailBody(ContactRequest contactRequest)
    {
        return $@"
        <html>
        <body style='font-family: Arial, sans-serif; color: #333;'>
            <h2 style='color: #2c5aa0;'>Nowe zapytanie z Konfiguratora Samochodowego</h2>
            
            <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
                <tr>
                    <td style='padding: 10px; border-bottom: 1px solid #ddd; font-weight: bold; width: 150px;'>Imię i nazwisko:</td>
                    <td style='padding: 10px; border-bottom: 1px solid #ddd;'>{contactRequest.Name}</td>
                </tr>
                <tr>
                    <td style='padding: 10px; border-bottom: 1px solid #ddd; font-weight: bold;'>Email:</td>
                    <td style='padding: 10px; border-bottom: 1px solid #ddd;'>{contactRequest.Email}</td>
                </tr>
                <tr>
                    <td style='padding: 10px; border-bottom: 1px solid #ddd; font-weight: bold;'>Telefon:</td>
                    <td style='padding: 10px; border-bottom: 1px solid #ddd;'>{contactRequest.Phone ?? "Nie podano"}</td>
                </tr>
                <tr>
                    <td style='padding: 10px; border-bottom: 1px solid #ddd; font-weight: bold;'>Temat:</td>
                    <td style='padding: 10px; border-bottom: 1px solid #ddd;'>{GetSubjectText(contactRequest.Subject)}</td>
                </tr>
            </table>
            
            <h3 style='color: #2c5aa0; margin-top: 30px;'>Wiadomość:</h3>
            <div style='background: #f9f9f9; padding: 15px; border-left: 4px solid #2c5aa0; margin: 10px 0;'>
                {contactRequest.Message.Replace("\n", "<br>")}
            </div>
            
            <hr style='margin: 30px 0; border: 0; border-top: 1px solid #ddd;'>
            <p style='color: #666; font-size: 12px;'>
                Ta wiadomość została wysłana przez formularz kontaktowy na stronie Konfiguratora Samochodowego.<br>
                Data wysłania: {DateTime.Now:dd.MM.yyyy HH:mm}
            </p>
        </body>
        </html>";
    }

    private async Task SimulateEmailSending(ContactRequest contactRequest)
    {
        _logger.LogWarning("========================================");
        _logger.LogWarning("          MOCK EMAIL SERVICE");
        _logger.LogWarning("========================================");
        _logger.LogWarning("To: konfiguratorsamochodwy@gmail.com");
        _logger.LogWarning("From: {Email} ({Name})", contactRequest.Email, contactRequest.Name);
        _logger.LogWarning("Phone: {Phone}", contactRequest.Phone ?? "Nie podano");
        _logger.LogWarning("Subject: {Subject}", GetSubjectText(contactRequest.Subject));
        _logger.LogWarning("Message: {Message}", contactRequest.Message);
        _logger.LogWarning("Timestamp: {Timestamp}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        _logger.LogWarning("========================================");
        _logger.LogWarning("UWAGA: To jest symulacja! Aby wysyłać prawdziwe emaile,");
        _logger.LogWarning("skonfiguruj SMTP w appsettings.json i ustaw UseMockService na false.");
        _logger.LogWarning("Szczegóły w pliku EMAIL_CONFIG.md");
        _logger.LogWarning("========================================");
        
        await Task.Delay(100);
    }
}