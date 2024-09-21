using Dominio.Entidades;
using Dominio.Interfaces;
using Infraestrutura.Dados.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Dominio.Dtos;
using System.Web.Helpers;

namespace Infraestrutura.Dados.Services
{
    public class GrupoTarefasServico
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public GrupoTarefasServico(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<GrupoTarefasDtoRetorno>?> GetGruposTarefaAsync(ClaimsPrincipal user)
        {
            var usuarioAtual = await _userManager.Users
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));

            if (usuarioAtual == null) return null;

            var spec = new Specification<GrupoTarefas>
            {
                Criteria = gt => gt.IdUsuario == usuarioAtual.Id,
                Includes = { gt => gt.Tarefas },
            };

            var gruposTarefas = await _unitOfWork.Repository<GrupoTarefas>().GetTudoAsync(spec);

            if (gruposTarefas == null)
                return null;
            
            var gruposTarefasDtoRetorno = gruposTarefas.Select(gt => new GrupoTarefasDtoRetorno
            {
                Id = gt.Id,
                Nome = gt.Nome,
                Criacao = gt.Criacao,
                Modificacao = gt.Modificacao,
                Tarefas = gt.Tarefas.Select(t => new TarefaDtoRetorno
                {
                    Id = t.Id,
                    Criacao = t.Criacao,
                    Modificacao = t.Modificacao,
                    Nome = t.Nome,
                    Descricao = t.Descricao,
                    Status = t.Status,
                    DataVencimento = t.DataVencimento,
                    GrupoTarefasId = t.GrupoTarefasId,
                    Conclusao = t.Conclusao
                }).ToList()
            }).ToList();

            return gruposTarefasDtoRetorno;
        }

        public async Task<GrupoTarefasDtoRetorno?> GetGrupoTarefasAsync(ClaimsPrincipal user, int GrupoTarefasId)
        {
            var usuarioAtual = await _userManager.Users
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));

            if (usuarioAtual == null) return null;

            var spec = new Specification<GrupoTarefas>
            {
                Criteria = gt => gt.IdUsuario == usuarioAtual.Id,
                Includes = { gt => gt.Tarefas }
            };

            var grupoTarefa = await _unitOfWork.Repository<GrupoTarefas>().GetPorIdAsync(GrupoTarefasId, spec);

            if (grupoTarefa == null)
                return null;
            
            var grupoTarefaDtoRetorno = new GrupoTarefasDtoRetorno
            {
                Nome = grupoTarefa.Nome,
                Criacao = grupoTarefa.Criacao,
                Modificacao = grupoTarefa.Modificacao,
                Tarefas = grupoTarefa.Tarefas.Select(t => new TarefaDtoRetorno
                {
                    Id = t.Id,
                    Criacao = t.Criacao,
                    Modificacao = t.Modificacao,
                    Nome = t.Nome,
                    Descricao = t.Descricao,
                    Status = t.Status,
                    DataVencimento = t.DataVencimento,
                    GrupoTarefasId = t.GrupoTarefasId,
                    Conclusao = t.Conclusao
                }).ToList()
            };

            return grupoTarefaDtoRetorno;
        }

        public async Task<bool> AddGrupoTarefasAsync(ClaimsPrincipal user, GrupoTarefasDtoPost grupoTarefaDto)
        {

            var usuarioAtual = await _userManager.Users
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));

            if (usuarioAtual == null)
                return false;

            var grupoTarefa = new GrupoTarefas
            {
                Nome = grupoTarefaDto.Nome,
                IdUsuario = usuarioAtual.Id,
                Criacao = DateTime.Now,
            };
            
            grupoTarefa.IdUsuario = usuarioAtual.Id;
            
            _unitOfWork.Repository<GrupoTarefas>().Add(grupoTarefa);

            return true;
        }

        public async Task<bool> DeleteGrupoTarefasAsync(int GrupoTarefasId)
        {
            var grupoTarefa = await _unitOfWork.Repository<GrupoTarefas>().GetPorIdAsync(GrupoTarefasId);

            if (grupoTarefa == null)
                return false;

            _unitOfWork.Repository<GrupoTarefas>().Delete(grupoTarefa);

            return true;
        }

        public async Task<bool> UpdateGrupoTarefasAsync(GrupoTarefasDtoPost updatedGrupoTarefasDto)
        {

            var grupoTarefaExistente = await _unitOfWork.Repository<GrupoTarefas>()
                .GetPorIdAsync((int) updatedGrupoTarefasDto.Id);

            if (grupoTarefaExistente == null)
                return false;

            grupoTarefaExistente.Nome = updatedGrupoTarefasDto.Nome;
            grupoTarefaExistente.Modificacao = DateTime.Now;

            _unitOfWork.Repository<GrupoTarefas>().Update(grupoTarefaExistente);

            return true;
        }
    }
}
