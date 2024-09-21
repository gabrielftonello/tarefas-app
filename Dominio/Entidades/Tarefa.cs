using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades
{
    public class Tarefa : EntidadeBase
    {
        public string Descricao { get; set; }
        public bool Status { get; set; }
        public DateTime? DataVencimento { get; set; }

        public int GrupoTarefasId { get; set; }
        
        [ForeignKey("GrupoTarefasId")]
        public virtual GrupoTarefas GrupoTarefas { get; set; }

        public DateTime? Conclusao { get; set; }
        public DateTime? Modificacao { get; set; }
    }
}