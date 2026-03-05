namespace GBastos.Casa_dos_Farelos.Messaging.RabbitMqPublisher;

public sealed class RabbitMqOptions
{
    public string Host { get; init; } = "localhost";
    public int Port { get; init; } = 5672;
    public string Username { get; init; } = "guest";
    public string Password { get; init; } = "guest";
    public string ExchangeName { get; init; } = "casa_dos_farelos_exchange";
    public string QueuePrefix { get; init; } = "cdf";
}