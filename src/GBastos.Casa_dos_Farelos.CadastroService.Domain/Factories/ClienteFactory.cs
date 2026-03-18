using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Services;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Factories;

public static class ClienteFactory
{
    public static Cliente Criar(
        TipoCliente tipo,
        string nome,
        string telefone,
        string email,
        string documento)
    {
        return tipo switch
        {
            TipoCliente.PessoaFisica =>
                ClientePessoaFisica.Criar(
                    nome,
                    telefone,
                    email,
                    documento),

            TipoCliente.PessoaJuridica =>
                ClientePessoaJuridica.Criar(
                    nome,
                    telefone,
                    email,
                    documento),

            _ => throw new InvalidOperationException("Tipo de cliente inválido.")
        };
    }
}