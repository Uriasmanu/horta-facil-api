using horta_facil_api.Service;
using Microsoft.AspNetCore.Mvc;

namespace horta_facil_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PlantaController : ControllerBase
    {
        private readonly PlantaService _plantaService;

        public PlantaController(PlantaService plantaService)
        {
            _plantaService = plantaService;
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
    }
}