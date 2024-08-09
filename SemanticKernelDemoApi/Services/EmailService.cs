using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Options;
using SemanticKernelDemoApi.Settings;

namespace SemanticKernelDemoApi.Services;

// Sample email service utilizing Azure Communication Service for sending emails.
public class EmailService
{
    private readonly EmailClient _emailClient;
    private readonly ILogger<EmailService> _logger;
    private readonly EmailSettings _settings;

    public EmailService(ILogger<EmailService> logger, IOptionsMonitor<EmailSettings> options)
    {
        _emailClient = new EmailClient(options.CurrentValue.ConnectionString);
        _logger = logger;
        _settings = options.CurrentValue;
    }

    public async Task<bool> SendAsync(string subject, string body, string recipient)
    {
        try
        {
            var emailSendOperation = await _emailClient.SendAsync(
                WaitUntil.Completed,
                _settings.Sender,
                recipient,
                subject,
                string.Empty,
                body);
            EmailSendResult statusMonitor = emailSendOperation.Value;

            _logger.LogInformation($"Email send operation {emailSendOperation.Id} executed with result: {statusMonitor.Status}");

            return true;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");

            return false;
        }
    }
}
