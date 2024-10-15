using System.Text.Json.Serialization;

namespace horta_facil_api.DTOs
{
    public class PlantaDTO
    {
        public Guid Id { get; set; }
        public string NomePlanta { get; set; }
        public int DiasParaColheita { get; set; }
        public bool Ativo { get; set; } = true;

        [JsonIgnore]
        public DateTime DiaDoPlantio { get; set; }

        public string DiaPlantioFormatado => DiaDoPlantio.ToString("dd/MM");

        public PlantaDTO() { }

        public PlantaDTO( DateTime diaDoPlantio) 
        { 
            Id = Guid.NewGuid();
            DiaDoPlantio = diaDoPlantio;
        }
    }
}
