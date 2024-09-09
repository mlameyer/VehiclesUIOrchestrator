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
        
        public async Task<IList<VehicleByTypeAndOrManufacturerDto>> GetVehiclesByVehicleTypeAndOrManufacturerId(string? vehicleType, int? manufacturerId)
        {
            _logger.LogInformation("Calling navixCaseStudyRepository.GetListOfAllVehiclesAsync()");
            Vehicles vehicles = await _navixCaseStudyRepository.GetListOfAllVehiclesAsync();

            IQueryable<Result> filteredResults = FilterVehicleResults(vehicles, vehicleType, manufacturerId);
            return MapVehicleResultsToVehicleByTypeAndOrManufacturerDto(filteredResults);
        }

        private IList<VehicleByTypeAndOrManufacturerDto> MapVehicleResultsToVehicleByTypeAndOrManufacturerDto(IQueryable<Result> filteredResults)
        {
            Dictionary<string, VehicleByTypeAndOrManufacturerDto> results = new Dictionary<string, VehicleByTypeAndOrManufacturerDto>();

            try
            {
                foreach (var r in filteredResults)
                {
                    if (results.ContainsKey(r.VehicleTypes.FirstOrDefault().Name))
                    {
                        results[r.VehicleTypes.FirstOrDefault().Name].Manufacturers.Add(new Manufacturer()
                        {
                            FullName = r.Mfr_Name,
                            Shortname = r.Mfr_CommonName,
                            Id = r.Mfr_ID,
                            Country = r.Country,
                        });
                    }
                    else
                    {
                        results.Add(r.VehicleTypes.FirstOrDefault().Name, new VehicleByTypeAndOrManufacturerDto()
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
            }
            catch (Exception ex)
            {
                _logger.LogError("Error attempting to map Vehicle to VehicleByTypeAndOrManufacturerDto: {0}", ex);
                throw;
            }

            return results.Values.ToList();
        }

        // IQueryable<t> gets evaluated at compile time. IEnumerable gets evaluated at execution time. There is better
        // performance using IQueryable because it can be evaluated and optimized by the compiler before execution.
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
