using Dominio.Entidades;
using Dominio.Interfaces;
using Infraestrutura.Dados.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Dominio.Dtos;

namespace Infraestrutura.Dados.Services
{
    public class TarefaServico
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public TarefaServico(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<TarefaDtoRetorno>?> GetTarefasAsync(ClaimsPrincipal user)
        {
            var usuarioAtual = await _userManager.Users
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));

            if (usuarioAtual == null) return null;

            var spec = new Specification<Tarefa>
            {
                Criteria = t => t.IdUsuario == usuarioAtual.Id
            };

            var tarefas = await _unitOfWork.Repository<Tarefa>().GetTudoAsync(spec);

            if (tarefas == null)
                return null;
            
            var tarefasDtoRetorno = tarefas.Select(t => new TarefaDtoRetorno
            {
                Id = t.Id,
                Criacao = t.Criacao,
                Modificacao = t.Modificacao,
                Nome = t.Nome,
                Descricao = t.Descricao,
                Status = t.Status,
                DataVencimento = t.DataVencimento,
                GrupoTarefasId = t.GrupoTarefasId,
                Conclusao = t.Conclusao,
            }).ToList();

            return tarefasDtoRetorno;
        }

        public async Task<TarefaDtoRetorno?> GetTarefaAsync(ClaimsPrincipal user, int idTarefa)
        {
            var usuarioAtual = await _userManager.Users
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));

            if (usuarioAtual == null) return null;

            var spec = new Specification<Tarefa>
            {
                Criteria = t => t.IdUsuario == usuarioAtual.Id,
            };

            var tarefa = await _unitOfWork.Repository<Tarefa>().GetPorIdAsync(idTarefa, spec);
            
            if (tarefa == null)
                return null;

            var tarefaDtoRetorno = new TarefaDtoRetorno
            {
                Id = tarefa.Id,
                Criacao = tarefa.Criacao,
                Modificacao = tarefa.Modificacao,
                Nome = tarefa.Nome,
                Descricao = tarefa.Descricao,
                Status = tarefa.Status,
                DataVencimento = tarefa.DataVencimento,
                GrupoTarefasId = tarefa.GrupoTarefasId,
                Conclusao = tarefa.Conclusao
            };
            
            return tarefaDtoRetorno;
        }

        public async Task<bool> AddTarefaAsync(ClaimsPrincipal user, TarefaDtoPost tarefaDto)
        {
            var usuarioAtual = await _userManager.Users
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));

            if (usuarioAtual == null)
                return false;

            if (await _unitOfWork.Repository<GrupoTarefas>().GetPorIdAsync(tarefaDto.GrupoTarefasId) == null)
            {
                return false;
            }
            
            var tarefa = new Tarefa
            {
                Nome = tarefaDto.Nome,
                Criacao = DateTime.Now,
                Descricao = tarefaDto.Descricao,
                Status = tarefaDto.Status,
                GrupoTarefasId = tarefaDto.GrupoTarefasId,
                DataVencimento = tarefaDto.DataVencimento,
                IdUsuario = usuarioAtual.Id,
            };
            
            _unitOfWork.Repository<Tarefa>().Add(tarefa);
            return true;
        }

        public async Task<bool> DeleteTarefaAsync(int idTarefa)
        {
            var tarefa = await _unitOfWork.Repository<Tarefa>().GetPorIdAsync(idTarefa);
            
            if (tarefa == null)
                return false;

            _unitOfWork.Repository<Tarefa>().Delete(tarefa);
            return true;
        }

        public async Task<bool> UpdateTarefaAsync(TarefaDtoPost updatedTarefaDto)
        {
 
            var tarefaExistente = await _unitOfWork.Repository<Tarefa>()
                .GetPorIdAsync((int) updatedTarefaDto.Id);

            if (tarefaExistente == null)
                return false;
            
            if (await _unitOfWork.Repository<GrupoTarefas>().GetPorIdAsync(updatedTarefaDto.GrupoTarefasId) == null)
            {
                return false;
            }

            tarefaExistente.Nome = updatedTarefaDto.Nome;
            tarefaExistente.Descricao = updatedTarefaDto.Descricao;
            tarefaExistente.Status = updatedTarefaDto.Status;
            tarefaExistente.DataVencimento = updatedTarefaDto.DataVencimento;
            tarefaExistente.Conclusao = null;
            
            if(updatedTarefaDto.Conclusao != null)
                tarefaExistente.Conclusao = ((DateTime)updatedTarefaDto.Conclusao).AddHours(-3);
            
            tarefaExistente.Modificacao = DateTime.Now;
            tarefaExistente.GrupoTarefasId = updatedTarefaDto.GrupoTarefasId;

            _unitOfWork.Repository<Tarefa>().Update(tarefaExistente);
            return true;
        }
    }
}
