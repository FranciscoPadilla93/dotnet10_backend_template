using System.Threading.Channels;
using LUPA.Api.Common.Email;

namespace LUPA.Api.Services.Email;

/// <summary>
/// Cola en memoria (System.Threading.Channels). Es Singleton a propósito: un solo canal
/// compartido por toda la app. EmailBackgroundService la lee en un loop continuo.
///
/// LIMITACIÓN CONOCIDA: si la app se reinicia con correos aún en cola, esos correos se
/// pierden (no es una cola persistida en disco/DB). Para un template esto es aceptable;
/// si algún día se vuelve crítico, la alternativa es una tabla "OutboxEmails" + un job
/// que la procese, o Hangfire.
/// </summary>
public class EmailQueue : IEmailService
{
    private readonly Channel<EmailMessage> _channel = Channel.CreateUnbounded<EmailMessage>();

    public ValueTask QueueEmailAsync(EmailMessage message)
    {
        return _channel.Writer.WriteAsync(message);
    }

    public IAsyncEnumerable<EmailMessage> ReadAllAsync(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}