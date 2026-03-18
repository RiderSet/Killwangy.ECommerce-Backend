namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;

public interface IEventSerializer
{
    string Serialize<T>(T message);
    T Deserialize<T>(string message);
}