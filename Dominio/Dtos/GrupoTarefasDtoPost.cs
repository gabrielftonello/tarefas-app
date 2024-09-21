using System.ComponentModel.DataAnnotations;

namespace Dominio.Dtos;

public class GrupoTarefasDtoPost
{
    public int? Id { get; set; }
    [Required(ErrorMessage = "O campo Nome � obrigat�rio.")]
    public string Nome { get; set; }
}