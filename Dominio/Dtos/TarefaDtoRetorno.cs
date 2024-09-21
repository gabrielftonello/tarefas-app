namespace Dominio.Dtos;

public class TarefaDtoRetorno
{
    public int? Id { get; set; }
    public DateTime Criacao { get; set; }
    public DateTime? Modificacao { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public required bool Status { get; set; }
    public DateTime? DataVencimento { get; set; }
    public required int GrupoTarefasId { get; set; }
    public DateTime? Conclusao { get; set; }
}