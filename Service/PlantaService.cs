using horta_facil_api.Data;
using horta_facil_api.DTOs;
using horta_facil_api.Models;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<bool> RegistrarPlantas ([FromBody] Plantas novaPlanta)
        {

            if (novaPlanta == null) 
            { 
                return false;// Não pode registrar uma planta nula
            }

            var plantaDTO = new PlantaDTO(novaPlanta.DiaDoPlantio);

            try
            {
                await _plantas.InsertOneAsync(novaPlanta);
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
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

        public async Task<PlantaDTO> BuscarPlantaPorId(Guid id)
        {
            var plantas = await _plantas.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (plantas == null)
            {
                return null;
            }

            return new PlantaDTO(plantas.DiaDoPlantio)
            {
                Id = plantas.Id,
                NomePlanta = plantas.NomePlanta,
                DiasParaColheita = plantas.DiasParaColheita,
            };
        }

        public async Task<bool> DeletarPlantaPorId(Guid id)
        {
            var resultado = await _plantas.DeleteOneAsync(x => x.Id == id);
            return resultado.DeletedCount > 0;
        }
    }
}
