using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Dados.Contexts;

public class TarefasContext(DbContextOptions<TarefasContext> options) : DbContext(options)
{
    public DbSet<Tarefa> Tarefas { get; set; }
    public DbSet<GrupoTarefas> GruposTarefas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}