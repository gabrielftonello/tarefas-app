using System.ComponentModel.DataAnnotations;

namespace Dominio.Dtos;

public class TarefaDtoPost
{
    public int? Id { get; set; }
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string Nome { get; set; }
    [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
    public string Descricao { get; set; }
    [Required(ErrorMessage = "O campo Status é obrigatório.")]
    public bool Status { get; set; }
    [Required(ErrorMessage = "O campo GrupoTarefasId é obrigatório.")]
    public int GrupoTarefasId { get; set; }
    public DateTime? DataVencimento { get; set; }
    public DateTime? Conclusao { get; set; }

}