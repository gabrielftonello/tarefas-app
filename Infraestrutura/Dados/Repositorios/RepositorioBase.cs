using Dominio.Entidades;
using Dominio.Interfaces;
using Infraestrutura.Dados.Contexts;
using Infraestrutura.Dados.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Dados.Repositorios
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : EntidadeBase
    {
        protected readonly TarefasContext _context;

        public RepositorioBase(TarefasContext context)
        {
            _context = context;
        }

        public async Task<T> GetPorIdAsync(int id, ISpecification<T> spec = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (spec != null)
            {
                query = AplicarSpecification(query, spec);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IReadOnlyList<T>> GetTudoAsync(ISpecification<T> spec = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (spec != null)
            {
                query = AplicarSpecification(query, spec);
            }

            return await query.ToListAsync();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        private IQueryable<T> AplicarSpecification(IQueryable<T> query, ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(query, spec);
        }

    }
}
