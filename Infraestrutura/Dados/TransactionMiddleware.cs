
using Dominio.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infraestrutura.Dados
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
        {
            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await _next(context);
                    await unitOfWork.CompleteAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }

}
