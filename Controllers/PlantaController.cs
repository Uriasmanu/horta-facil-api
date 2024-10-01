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
    }
}