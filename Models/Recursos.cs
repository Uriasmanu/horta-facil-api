namespace horta_facil_api.Models
{
    public class Recursos
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public string TipoRecurso { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}