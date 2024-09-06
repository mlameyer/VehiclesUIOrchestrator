using VehiclesUIOrchestrator.api.Dtos;
using VehiclesUIOrchestrator.Managers.Models;
using VehiclesUIOrchestrator.Repositories;

namespace VehiclesUIOrchestrator.Managers
{
    public class VehiclesManager: IVehiclesManager
    {
        private readonly ILogger<VehiclesManager> _logger;
        private readonly INavixCaseStudyRepository _navixCaseStudyRepository;

        public VehiclesManager(ILogger<VehiclesManager> logger, INavixCaseStudyRepository navixCaseStudyRepository)
        {
            _logger = logger;
            _navixCaseStudyRepository = navixCaseStudyRepository;
        }
        
        public async Task<IEnumerable<VehicleByTypeAndOrManufacturerDto>> GetVehiclesByVehicleTypeAndOrManufacturerId(string? vehicleType, int? manufacturerId)
        {
            Vehicles vehicles = await _navixCaseStudyRepository.GetListOfAllVehiclesAsync();

            IQueryable<Result> filteredResults = FilterVehicleResults(vehicles, vehicleType, manufacturerId);
            return MapVehicleResultsToVehicleByTypeAndOrManufacturerDto(filteredResults);
        }

        private IEnumerable<VehicleByTypeAndOrManufacturerDto> MapVehicleResultsToVehicleByTypeAndOrManufacturerDto(IQueryable<Result> filteredResults)
        {
            List<VehicleByTypeAndOrManufacturerDto> results = new List<VehicleByTypeAndOrManufacturerDto>();
            Dictionary<string, VehicleByTypeAndOrManufacturerDto> vehicleTypes = new Dictionary<string, VehicleByTypeAndOrManufacturerDto>();

            foreach (var r in filteredResults)
            {
                if (vehicleTypes.ContainsKey(r.VehicleTypes.FirstOrDefault().Name))
                {
                    vehicleTypes[r.VehicleTypes.FirstOrDefault().Name].Manufacturers.Add(new Manufacturer()
                    {
                        FullName = r.Mfr_Name,
                        Shortname = r.Mfr_CommonName,
                        Id = r.Mfr_ID,
                        Country = r.Country,
                    });
                }
                else
                {
                    vehicleTypes.Add(r.VehicleTypes.FirstOrDefault().Name, new VehicleByTypeAndOrManufacturerDto()
                    {
                        VehicleType = r.VehicleTypes.FirstOrDefault().Name,
                        Manufacturers = new List<Manufacturer>() { new Manufacturer() {
                                FullName = r.Mfr_Name,
                                Shortname = r.Mfr_CommonName,
                                Id = r.Mfr_ID,
                                Country = r.Country,
                            } }
                    });
                }
            }

            return results = vehicleTypes.Values.ToList();
        }

        private static IQueryable<Result> FilterVehicleResults(Vehicles vehicles, string? vehicleType, int? manufacturerId)
        {
            // Strategy pattern might be a good option
            if (vehicleType != null && manufacturerId != null)
            {
                return vehicles.Results.AsQueryable().Where(r => r.Mfr_ID == manufacturerId && r.VehicleTypes.Any(x => x.IsPrimary && x.Name == vehicleType));
            }
            else if (vehicleType != null && manufacturerId == null)
            {
                return vehicles.Results.AsQueryable().Where(v => v.VehicleTypes.Any(x => x.IsPrimary && x.Name == vehicleType));
            }
            else if (vehicleType == null && manufacturerId != null)
            {
                return vehicles.Results.AsQueryable().Where(r => r.Mfr_ID == manufacturerId && r.VehicleTypes.Any(x => x.IsPrimary));
            }
            else
            {
                return vehicles.Results.AsQueryable().Where(r => r.VehicleTypes.Any(x => x.IsPrimary));
            }
        }
    }
}
