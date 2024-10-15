namespace horta_facil_api.Models
{
    public class Plantas
    {
        public Guid Id { get; set; }
        public string NomePlanta { get; set; }
        public DateTime DiaDoPlantio { get; set; } = DateTime.Now;
        public int DiasParaColheita { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
