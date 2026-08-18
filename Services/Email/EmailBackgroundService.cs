using LUPA.Api.Common.Email;
using LUPA.Api.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;

namespace LUPA.Api.Services.Email;

public class EmailBackgroundService : BackgroundService
{
    private readonly EmailQueue _queue;
    private readonly IOptions<EmailOptions> _emailOptions;
    private readonly ILogger<EmailBackgroundService> _logger;

    public EmailBackgroundService(
        EmailQueue queue,
        IOptions<EmailOptions> emailOptions,
        ILogger<EmailBackgroundService> logger)
    {
        _queue = queue;
        _emailOptions = emailOptions;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in _queue.ReadAllAsync(stoppingToken))
        {
            try
            {
                await SendAsync(message, stoppingToken);
            }
            catch (Exception ex)
            {
                // Un correo fallido no debe tumbar el background service completo:
                // se loguea y se sigue procesando la cola.
                _logger.LogError(ex, "Error enviando correo a {To}", message.To);
            }
        }
    }

    private async Task SendAsync(EmailMessage message, CancellationToken cancellationToken)
    {
        var options = _emailOptions.Value;

        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(options.FromName, options.FromEmail));
        mimeMessage.To.Add(MailboxAddress.Parse(message.To));
        mimeMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder();

        if (message.IsHtml)
        {
            bodyBuilder.HtmlBody = message.Body;
        }
        else
        {
            bodyBuilder.TextBody = message.Body;
        }

        mimeMessage.Body = bodyBuilder.ToMessageBody();

        using var client = new MailKit.Net.Smtp.SmtpClient();

        await client.ConnectAsync(
            options.Host,
            options.Port,
            options.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None,
            cancellationToken);

        if (!string.IsNullOrWhiteSpace(options.Username))
        {
            await client.AuthenticateAsync(options.Username, options.Password, cancellationToken);
        }

        await client.SendAsync(mimeMessage, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}