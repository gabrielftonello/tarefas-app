using System.ComponentModel.DataAnnotations;

namespace Dominio.Dtos
{
    public class LoginDtoPost
    {
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email está inválido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Password é obrigatório.")]
        public string Password { get; set; }
    }
}
