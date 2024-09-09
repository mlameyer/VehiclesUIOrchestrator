using VehiclesUIOrchestrator.Managers.Models;

namespace VehiclesUIOrchestrator.Repositories
{
    public class NavixCaseStudyRepository : INavixCaseStudyRepository
    {
        private readonly ILogger<NavixCaseStudyRepository> _logger;
        private static HttpClient httpClient;
        public NavixCaseStudyRepository(ILogger<NavixCaseStudyRepository> logger) 
        {
            _logger = logger;
            httpClient = new()
            {
                BaseAddress = new Uri("https://navixrecruitingcasestudy.blob.core.windows.net")
            };
        }

        public async Task<Vehicles> GetListOfAllVehiclesAsync()
        {
            Vehicles? response = new Vehicles();
            _logger.LogInformation("Calling {0}", httpClient.BaseAddress);
            try
            {
                response = await httpClient.GetFromJsonAsync<Vehicles>("/manufacturers/vehicle-manufacturers.json");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error calling {0}: {1}", httpClient.BaseAddress, ex);
            }
            

            return response;
        }
    }
}
