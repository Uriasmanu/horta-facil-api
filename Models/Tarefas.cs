namespace horta_facil_api.Models
{
    public class Tarefas
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public StatusTarefa Status { get; set; } // Enum para o status da tarefa
        public DateTime Data { get; set; }
        public Voluntarios Voluntario { get; set; }
    }

    // Enum para os diferentes status de uma tarefa
    public enum StatusTarefa
    {
        Pendente,      // Tarefa ainda não iniciada
        EmAndamento,   // Tarefa em progresso
        Concluida      // Tarefa finalizada
    }
}
