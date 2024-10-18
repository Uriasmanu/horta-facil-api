namespace horta_facil_api.Models
{
    public class Voluntarios
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public Tarefas Tarefa { get; set; }
    }

}