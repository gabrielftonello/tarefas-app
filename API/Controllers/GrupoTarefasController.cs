using API.Erros;
using Dominio.Dtos;
using Dominio.Entidades;
using Infraestrutura.Dados.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{


    public class GrupoTarefasController : BaseApiController
    {
        private readonly GrupoTarefasServico _grupoTarefaServico;

        public GrupoTarefasController(GrupoTarefasServico grupoTarefaServico)
        {
            _grupoTarefaServico = grupoTarefaServico;
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllGrupoTarefas()
        {
            var grupos = await _grupoTarefaServico.GetGruposTarefaAsync(User);
            
            if (grupos == null)
                return NotFound();

            return Ok(grupos);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGrupoTarefas(int id)
        {
            var grupo = await _grupoTarefaServico.GetGrupoTarefasAsync(User, id);
            
            if (grupo == null)
                return NotFound();

            return Ok(grupo);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CriarGrupoTarefas([FromBody] GrupoTarefasDtoPost grupoTarefaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _grupoTarefaServico.AddGrupoTarefasAsync(User, grupoTarefaDto))
                return NoContent();
            
            return BadRequest(new ApiResposta(500));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrupoTarefas(GrupoTarefasDtoPost grupoTarefaDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _grupoTarefaServico.UpdateGrupoTarefasAsync(grupoTarefaDto))
                return NoContent();
            
            return BadRequest(new ApiResposta(500));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrupoTarefas(int id)
        {
            if(await _grupoTarefaServico.DeleteGrupoTarefasAsync(id))
                return NoContent();
                
            return BadRequest(new ApiResposta(500));
        }
    }
}
