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
            _plantas = context.Plantas;
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
                // Definir o filtro para buscar apenas plantas ativas
                var filtro = Builders<Plantas>.Filter.Eq(planta => planta.Ativo, true);

                // Aplicar o filtro ao buscar as plantas
                var plantas = await _plantas.Find(filtro).ToListAsync();

                // Mapear as plantas para DTOs
                var plantaDTO = plantas.Select(plantas => new PlantaDTO(plantas.DiaDoPlantio)
                {
                    Id = plantas.Id,
                    NomePlanta = plantas.NomePlanta,
                    DiasParaColheita = plantas.DiasParaColheita,
                    Ativo = plantas.Ativo // Adicionar o campo Ativo no DTO
                }).ToList();

                return plantaDTO;
            }
            catch (MongoException mongoEx)
            {
                // Tratamento específico para falhas no MongoDB
                Console.WriteLine($"Erro ao buscar plantas no banco de dados: {mongoEx.Message}");
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
                Ativo = plantas.Ativo
            };
        }

        public async Task<bool> DesativarPlantaPorId(Guid id)
        {
            // Buscar a planta no banco de dados com base no modelo 'Plantas'
            var planta = await _plantas.Find(x => x.Id == id).FirstOrDefaultAsync();

            // Verificar se a planta foi encontrada
            if (planta == null)
            {
                return false; // Planta não encontrada
            }

            // Definir o campo 'Ativo' como false
            planta.Ativo = false;

            // Atualizar o documento no banco de dados com o modelo 'Plantas' modificado
            var resultado = await _plantas.ReplaceOneAsync(
                Builders<Plantas>.Filter.Eq(x => x.Id, id),
                planta
            );

            // Retornar verdadeiro se pelo menos um documento foi atualizado
            return resultado.ModifiedCount > 0;
        }


    }
}
