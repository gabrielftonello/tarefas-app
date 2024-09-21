namespace API.Erros;

    public class ApiRespostaErroValidacao : ApiResposta
    {
        public ApiRespostaErroValidacao() : base(400)
        {
        }

        public IEnumerable<string> Erros { get; set; }
    }

