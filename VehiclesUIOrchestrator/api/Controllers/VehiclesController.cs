using Microsoft.AspNetCore.Mvc;
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

        //[HttpGet(Name = "GetVehicles")]
        //public async Task<IEnumerable<VehicleByTypeAndOrManufacturerDto>> Get()
        //{
        //    return await _vehiclesManager.GetVehiclesByVehicleTypeAndOrManufacturerId("Passenger Car", null);
        //}

        [HttpGet(Name = "GetVehiclesByVehicleTypeAndManufacturerId")]
        public async Task<IEnumerable<VehicleByTypeAndOrManufacturerDto>> Get(string? vehicletype, int? manufacturerid)
        {
            return await _vehiclesManager.GetVehiclesByVehicleTypeAndOrManufacturerId(vehicletype, manufacturerid);
        }
    }
}
