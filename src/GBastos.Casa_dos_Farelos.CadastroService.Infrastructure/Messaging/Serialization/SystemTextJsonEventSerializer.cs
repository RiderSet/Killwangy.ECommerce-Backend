using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Messaging.Serialization;

public sealed class SystemTextJsonEventSerializer : IEventSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public string Serialize<T>(T message)
    {
        return JsonSerializer.Serialize(message, Options);
    }

    public T Deserialize<T>(string message)
    {
        var result = JsonSerializer.Deserialize<T>(message, Options);

        if (result == null)
            throw new InvalidOperationException("Erro ao desserializar evento.");

        return result;
    }
}