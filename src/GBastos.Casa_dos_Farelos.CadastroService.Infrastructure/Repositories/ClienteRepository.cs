using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly CadastroDbContext _context;

    public ClienteRepository(CadastroDbContext context)
    {
        _context = context;
    }

    private Task<Cliente?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Clientes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return FindByIdAsync(id, cancellationToken);
    }

    public Task<Cliente?> GetByIdAsync(Guid clienteId, CancellationToken cancellationToken)
    {
        return FindByIdAsync(clienteId, cancellationToken);
    }

    public async Task AdicionarAsync(Cliente cliente, CancellationToken cancellationToken)
    {
        await _context.Clientes.AddAsync(cliente, cancellationToken);
    }

    public async Task<bool> EmailJaExisteAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Clientes.AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<bool> ExistePorDocumentoAsync(string documento, CancellationToken cancellationToken)
    {
        return await _context.Clientes.AnyAsync(x => x.Documento == documento, cancellationToken);
    }

    public void Remover(Cliente cliente)
    {
        _context.Clientes.Remove(cliente);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}