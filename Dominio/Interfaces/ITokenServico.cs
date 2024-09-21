
using Microsoft.AspNetCore.Identity;

namespace Dominio.Interfaces;

    public interface ITokenServico
    {
        string CriarToken(IdentityUser user);
    }

