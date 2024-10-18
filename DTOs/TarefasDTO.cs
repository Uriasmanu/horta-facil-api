using horta_facil_api.DTOs;

namespace horta_facil_api.Models
{
    public class TarefasDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public VoluntarioDTO Voluntario { get; set; }
    }
}