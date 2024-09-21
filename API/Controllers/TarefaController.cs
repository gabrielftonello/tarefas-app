using API.Erros;
using Dominio.Dtos;
using Infraestrutura.Dados.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class TarefaController : BaseApiController
{
    
       private readonly TarefaServico _tarefaServico;
    
            public TarefaController(TarefaServico tarefaServico)
            {
                _tarefaServico = tarefaServico;
            }
    
            [Authorize]
            [HttpGet]
            public async Task<IActionResult> GetAllTarefa()
            {
                var tarefas = await _tarefaServico.GetTarefasAsync(User);
                
                if (tarefas == null)
                    return NotFound(new ApiResposta(404));
    
                return Ok(tarefas);
            }
    
            [Authorize]
            [HttpGet("{id}")]
            public async Task<IActionResult> GetTarefa(int id)
            {
                var tarefa = await _tarefaServico.GetTarefaAsync(User, id);
                
                if (tarefa == null)
                    return NotFound(new ApiResposta(404));
    
                return Ok(tarefa);
            }
    
            [Authorize]
            [HttpPost]
            public async Task<IActionResult> CriarTarefa(TarefaDtoPost tarefa)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _tarefaServico.AddTarefaAsync(User, tarefa))
                {
                    return NoContent();
                }

                return BadRequest(new ApiResposta(500));
            }
    
            [Authorize]
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateTarefa(int id, TarefaDtoPost tarefa)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _tarefaServico.UpdateTarefaAsync(tarefa))
                {
                    return NoContent();
                }
                
                return BadRequest(new ApiResposta(500));
            }
    
            [Authorize]
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteTarefa(int id)
            {
                if (await _tarefaServico.DeleteTarefaAsync(id))
                {
                    return NoContent();
                }
                
                return BadRequest(new ApiResposta(500));
            }
}