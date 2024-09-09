using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using VehiclesUIOrchestrator.api.Dtos;
using VehiclesUIOrchestrator.Managers;

namespace VehiclesUIOrchestrator.api.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly ILogger<VehiclesController> _logger;
        private readonly IVehiclesManager _vehiclesManager;

        public VehiclesController(ILogger<VehiclesController> logger, IVehiclesManager vehiclesManager)
        {
            _logger = logger;
            _vehiclesManager = vehiclesManager;
        }

        [HttpGet(Name = "GetVehiclesByVehicleTypeAndManufacturerId")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string? vehicletype, int? manufacturerid)
        {
            IList<VehicleByTypeAndOrManufacturerDto> result 
                = await _vehiclesManager.GetVehiclesByVehicleTypeAndOrManufacturerId(vehicletype, manufacturerid);

            if(result.Count() == 0)
            {
                return NoContent();
            }

            return Ok(result);
        }
    }
}
