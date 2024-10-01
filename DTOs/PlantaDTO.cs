namespace horta_facil_api.DTOs
{
    public class PlantaDTO
    {
        public Guid Id { get; set; }
        public string NomePlanta { get; set; }
        public int DiasParaColheita { get; set; }
        private DateTime DiaDoPlantio { get; set; }

        public string DiaPlantioFormatado => DiaDoPlantio.ToString("dd/MM");

        public PlantaDTO( DateTime diaDoPlantio) 
        { 
            DiaDoPlantio = diaDoPlantio;
        }
    }
}
