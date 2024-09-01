using VehiclesUIOrchestrator.api.Dtos;
using VehiclesUIOrchestrator.Managers.Models;
using VehiclesUIOrchestrator.Repositories;

namespace VehiclesUIOrchestrator.Managers
{
    public class VehiclesManager: IVehiclesManager
    {
        private readonly ILogger<VehiclesManager> logger;
        private readonly INavixCaseStudyRepository navixCaseStudyRepository;

        public VehiclesManager(ILogger<VehiclesManager> logger, INavixCaseStudyRepository navixCaseStudyRepository)
        {
            this.logger = logger;
            this.navixCaseStudyRepository = navixCaseStudyRepository;
        }
        
        public async Task<IEnumerable<VehicleByTypeAndOrManufacturerDto>> GetVehiclesByVehicleTypeAndOrManufacturerId(string? vehicleType, int? manufacturerId)
        {
            Vehicles vehicles = await navixCaseStudyRepository.GetListOfAllVehiclesAsync();

            IEnumerable<Result> filteredResults = FilterVehicleResults(vehicles, vehicleType, manufacturerId);
            IEnumerable<VehicleByTypeAndOrManufacturerDto> mappedResults = MapVehicleResults(filteredResults);

            return mappedResults;
        }

        private IEnumerable<VehicleByTypeAndOrManufacturerDto> MapVehicleResults(IEnumerable<Result> filteredResults)
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

        private static IEnumerable<Result> FilterVehicleResults(Vehicles vehicles, string? vehicleType, int? manufacturerId)
        {
            IEnumerable<Result> results = new List<Result>();

            // Strategy pattern might be a good option
            if (vehicleType != null && manufacturerId != null)
            {
                results = from s in vehicles.Results where s.Mfr_ID == manufacturerId && s.VehicleTypes.Where(x => x.IsPrimary == true).Select(x => x.Name).Contains(vehicleType) select s;
            }
            else if (vehicleType != null && manufacturerId == null)
            {
                results = from s in vehicles.Results where s.VehicleTypes.Where(x => x.IsPrimary == true).Select(x => x.Name).Contains(vehicleType) select s;
            }
            else if (vehicleType == null && manufacturerId != null)
            {
                results = from s in vehicles.Results where s.Mfr_ID == manufacturerId select s;
            }
            else
            {
                results = vehicles.Results.Where(x => x.VehicleTypes.FirstOrDefault().IsPrimary == true);
            }

            return results;
        }
    }
}
