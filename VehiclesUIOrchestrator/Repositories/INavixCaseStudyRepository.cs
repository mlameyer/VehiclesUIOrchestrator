using VehiclesUIOrchestrator.Managers.Models;

namespace VehiclesUIOrchestrator.Repositories
{
    public interface INavixCaseStudyRepository
    {
        public Task<Vehicles> GetListOfAllVehiclesAsync();
    }
}
