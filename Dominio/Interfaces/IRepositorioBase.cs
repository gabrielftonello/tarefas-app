using Dominio.Entidades;

namespace Dominio.Interfaces;

public interface IRepositorioBase<T> where T : EntidadeBase
{
    Task<T> GetPorIdAsync(int id, ISpecification<T> spec = null);
    Task<IReadOnlyList<T>> GetTudoAsync(ISpecification<T> spec = null);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}

