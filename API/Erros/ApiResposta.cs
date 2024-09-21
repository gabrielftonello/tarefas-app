namespace API.Erros
{
    public class ApiResposta
    {
        public ApiResposta(int statusCode, string mensagem = null)
        {
            StatusCode = statusCode;
            Mensagem = mensagem ?? RetornaMensagemPadraoPorStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Mensagem { get; set; }

        private string RetornaMensagemPadraoPorStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A solicitação feita ao servidor contém erros de formatação ou parâmetros inválidos. Verifique os dados enviados e tente novamente.",
                401 => "Acesso negado. Você não tem autorização para acessar este recurso. Por favor, faça login ou verifique suas credenciais.",
                404 => "O recurso solicitado não foi encontrado no servidor. Verifique o URL ou tente procurar novamente mais tarde.",
                500 => "Ocorreu um erro inesperado no servidor. Estamos trabalhando para resolver o problema. Por favor, tente novamente mais tarde.",
                _ => null
            };
        }
    }
}
