namespace horta_facil_api.Models
{
    public class Recursos
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Status { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
    }

}
