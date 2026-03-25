using GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;
using GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Clientes.ObterCliente;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Handlers;

public sealed class ObterClienteQueryHandler
    : IRequestHandler<ObterClienteQuery, ClienteDto?>
{
    private readonly CadastroDbContext _context;

    public ObterClienteQueryHandler(CadastroDbContext context)
    {
        _context = context;
    }

    public async Task<ClienteDto?> Handle(
        ObterClienteQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Clientes
            .AsNoTracking()
            .Where(c => c.Id == request.Id)
            .Select(c => new ClienteDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}