using Dominio.Entidades;
using Dominio.Interfaces;
using Infraestrutura.Dados.Contexts;
using Infraestrutura.Dados.Repositorios;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;

namespace Infraestrutura.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly TarefasContext _context;
    private Hashtable _repositories;

    public UnitOfWork(TarefasContext context)
    {
        _context = context;
    }

    public IRepositorioBase<T> Repository<T>() where T : EntidadeBase
    {
        if (_repositories == null)
            _repositories = new Hashtable();

        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositorioBase<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
            _repositories.Add(type, repositoryInstance);
        }

        return (IRepositorioBase<T>)_repositories[type];
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }


    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}


