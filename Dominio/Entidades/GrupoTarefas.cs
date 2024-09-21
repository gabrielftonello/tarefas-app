
namespace Dominio.Entidades;

    public class GrupoTarefas : EntidadeBase
    {
        public virtual ICollection<Tarefa> Tarefas { get; set; }
    }

