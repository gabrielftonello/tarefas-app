using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades;

    public class EntidadeBase
    { 
    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Nome { get; set; }
        public string IdUsuario { get; set; }
        public DateTime Criacao { get; set; }
        public DateTime? Modificacao { get; set; }
    }

