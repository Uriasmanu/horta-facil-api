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
                // Retorna falso e também pode lançar uma exceção customizada, se preferir
                throw new ArgumentNullException(nameof(novaPlanta), "A planta enviada está nula.");
            }

            var plantaDTO = new PlantaDTO(novaPlanta.DiaDoPlantio);

            try
            {
                await _plantas.InsertOneAsync(novaPlanta);
                return true;
            }
            catch (MongoWriteException mongoEx)
            {
                // Tratamento específico para falhas no MongoDB (ex: violação de unicidade, erros de gravação)
                Console.WriteLine($"Erro ao inserir a planta no banco de dados: {mongoEx.Message}");
                return false;
            }
            catch (TimeoutException timeoutEx)
            {
                // Tratamento específico para falhas de tempo de execução
                Console.WriteLine($"Tempo limite excedido ao tentar registrar a planta: {timeoutEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Tratamento genérico de exceções
                Console.WriteLine($"Ocorreu um erro inesperado ao registrar a planta: {ex.Message}");
                return false;
            }
        }

        public async Task<List<PlantaDTO>> BuscarTodasPlanta()
        {
            try
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
            catch (MongoException mongoEx)
            {
                // Tratamento específico para falhas no MongoDB
                Console.WriteLine($"Erro ao buscar plantas no banco de dados: {mongoEx.Message}");
                // Você pode retornar uma lista vazia ou lançar uma exceção dependendo do seu caso de uso
                return new List<PlantaDTO>();
            }
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
