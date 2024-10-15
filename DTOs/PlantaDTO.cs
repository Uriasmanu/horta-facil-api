using System.Text.Json.Serialization;

namespace horta_facil_api.DTOs
{
    public class PlantaDTO
    {
        public Guid Id { get; set; }
        public string NomePlanta { get; set; }
        public int DiasParaColheita { get; set; }
        public bool Ativo { get; set; } 

        [JsonIgnore]
        public DateTime DiaDoPlantio { get; set; }

        public string DiaPlantioFormatado => DiaDoPlantio.ToString("dd/MM");

        public PlantaDTO() { }

        public PlantaDTO( DateTime diaDoPlantio) 
        { 
            Id = Guid.NewGuid();
            DiaDoPlantio = diaDoPlantio;
            Ativo = true; // Definir o campo Ativo como true por padrão
        }

        // Método para desativar a planta
        public void Desativar()
        {
            Ativo = false; // Altera o campo Ativo para false
        }
    }
}
