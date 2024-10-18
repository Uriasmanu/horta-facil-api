namespace horta_facil_api.Models
{
    public class Tarefas
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Status { get; set; }
        public DateTime Data {  get; set; }
        public Voluntarios Voluntario { get; set; }
    }

}