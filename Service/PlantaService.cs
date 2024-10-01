using horta_facil_api.Data;
using horta_facil_api.DTOs;
using horta_facil_api.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace horta_facil_api.Service
{
    public class PlantaService
    {
        private readonly IMongoCollection<Plantas> _plantas;

        public PlantaService(MongoDbContext context)
        {
            _plantas = context.Planta;
        }

        public async Task<List<PlantaDTO>> BuscarTodasPlanta()
        {
            var plantas = await _plantas.Find(new BsonDocument()).ToListAsync();

            var plantaDTO = plantas.Select(plantas => new PlantaDTO(plantas.DiaDoPlantio)
            {
                Id = plantas.Id,
                NomePlanta = plantas.NomePlanta,
                DiasParaColheita = plantas.DiasParaColheita,
               
            }).ToList();

            return plantaDTO;
        }
    }
}
