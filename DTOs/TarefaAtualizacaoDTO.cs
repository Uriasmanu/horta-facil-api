namespace horta_facil_api.Models
{
    public class TarefaAtualizacaoDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Guid IdVoluntario { get; set; }
    }
}