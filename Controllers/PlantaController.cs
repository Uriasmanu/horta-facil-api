using horta_facil_api.DTOs;
using horta_facil_api.Models;
using horta_facil_api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace horta_facil_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlantaController : ControllerBase
    {
        private readonly PlantaService _plantaService;

        public PlantaController(PlantaService plantaService)
        {
            _plantaService = plantaService;
        }

        [HttpPost("plantas")]
         public async Task<ActionResult> Registrar([FromBody] PlantaDTO novaPlantaDTO)
         {
            var plantaModel = new Plantas
            {
                Id = Guid.NewGuid(),
                NomePlanta = novaPlantaDTO.NomePlanta,
                DiasParaColheita = novaPlantaDTO.DiasParaColheita,
                DiaDoPlantio = novaPlantaDTO.DiaDoPlantio // Agora você pode usar a data de plantio do DTO
            };

            var plantaRegistrada = await _plantaService.RegistrarPlantas(plantaModel);
            
            if (!plantaRegistrada)
            {
                return BadRequest("Não foi possivel registrar a planta");
            }

            return Ok("Planta registrada com sucesso");
         }

        [HttpGet("plantas")]
        public async Task<ActionResult> MostrarPlantas()
        {
            var plantas = await _plantaService.BuscarTodasPlanta();
            return Ok(plantas);
        }

        [HttpGet("plantas/{id}")]
        public async Task<ActionResult> BuscarPlantasPorId( string id)
        {
            var plantas = await _plantaService.BuscarPlantaPorId(System.Guid.Parse(id));

            if (plantas == null) 
            {
                return NotFound("Planta não encontrada");
            }
            return Ok(plantas);
        }

        [HttpDelete("plantas/{id}")]
        public async Task<ActionResult> DeletarPlantasPorId(string id)
        {
            var plantaDeletada = await _plantaService.DeletarPlantaPorId(System.Guid.Parse(id));

            if (!plantaDeletada)
            {
                return NotFound("Planta não encontrada ou ja deletada");
            }

            return Ok("planta deletada com sucesso");
        }
    }
}