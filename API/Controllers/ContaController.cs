using Dominio.Dtos;
using API.Erros;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ContaController : BaseApiController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenServico _tokenServico;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ContaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            ITokenServico tokenServico)
        {
            _signInManager = signInManager;
            _tokenServico = tokenServico;
            _userManager = userManager;
        }

        [HttpPost("registro")]
        public async Task<ActionResult<UsuarioDtoRetorno>> Registrar(RegistroDtoPost registroDtoPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (CheckEmailExistsAsync(registroDtoPost.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiRespostaErroValidacao
                { Erros = new[] { "Email em uso." } });
            }

            var user = new IdentityUser
            {
                Email = registroDtoPost.Email,
                UserName = registroDtoPost.Email
            };

            var result = await _userManager.CreateAsync(user, registroDtoPost.Password);

            if (!result.Succeeded) return BadRequest(new ApiResposta(400));

            return new UsuarioDtoRetorno
            {
                Email = user.Email,
                Token = _tokenServico.CriarToken(user),
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDtoRetorno>> Login(LoginDtoPost loginDtoPost)
        {
            var user = await _userManager.FindByEmailAsync(loginDtoPost.Email);

            if (user == null) return Unauthorized(new ApiResposta(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDtoPost.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResposta(401));

            return new UsuarioDtoRetorno
            {
                Email = user.Email,
                Token = _tokenServico.CriarToken(user),
            };
        }

        [HttpGet("emailexiste")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

    }
}
