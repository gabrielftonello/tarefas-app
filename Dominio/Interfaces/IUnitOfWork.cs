

using Dominio.Entidades;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dominio.Interfaces;

    public interface IUnitOfWork
    {
        IRepositorioBase<TEntity> Repository<TEntity>() where TEntity : EntidadeBase;
        Task<int> CompleteAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }

