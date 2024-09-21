
namespace Dominio.Dtos;

public class GrupoTarefasDtoRetorno
{
    public int? Id { get; set; }
    public string Nome { get; set; }
    public DateTime Criacao { get; set; }
    public DateTime? Modificacao { get; set; }
    public virtual ICollection<TarefaDtoRetorno> Tarefas { get; set; }
}