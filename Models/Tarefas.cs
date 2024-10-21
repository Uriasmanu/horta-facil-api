namespace horta_facil_api.Models
{
    public class Tarefas
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public string Descricao { get; set; }
        public Guid IdVoluntario { get; set; }

        // Status da tarefa
        private int _status; // 0: Pendente, 1: Em Progresso, 2: Finalizado
        public int Status
        {
            get { return _status; }
            private set
            {
                if (value < 0 || value > 2)
                    throw new ArgumentOutOfRangeException(nameof(value), "Status deve ser 0 (Pendente), 1 (Em Progresso) ou 2 (Finalizado).");
                _status = value;
            }
        }

        // Construtor padrão
        public Tarefas()
        {
            Status = 0; // Status inicial é Pendente
        }

        // Método para definir o status
        public void DefinirStatus(int novoStatus)
        {
            Status = novoStatus;
        }
    }
}
