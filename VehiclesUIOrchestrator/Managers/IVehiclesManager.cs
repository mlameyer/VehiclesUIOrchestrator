using VehiclesUIOrchestrator.api.Dtos;

namespace VehiclesUIOrchestrator.Managers
{
    public interface IVehiclesManager
    {
        public Task<IEnumerable<VehicleByTypeAndOrManufacturerDto>> GetVehiclesByVehicleTypeAndOrManufacturerId(string? vehicleType, int? manufacturerId);
    }
}