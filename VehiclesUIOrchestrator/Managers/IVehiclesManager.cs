using VehiclesUIOrchestrator.api.Dtos;

namespace VehiclesUIOrchestrator.Managers
{
    public interface IVehiclesManager
    {
        public Task<IList<VehicleByTypeAndOrManufacturerDto>> GetVehiclesByVehicleTypeAndOrManufacturerId(string? vehicleType, int? manufacturerId);
    }
}