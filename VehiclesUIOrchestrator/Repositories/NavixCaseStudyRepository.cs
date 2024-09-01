using VehiclesUIOrchestrator.Managers.Models;

namespace VehiclesUIOrchestrator.Repositories
{
    public class NavixCaseStudyRepository : INavixCaseStudyRepository
    {
        private static HttpClient httpClient;
        public NavixCaseStudyRepository() 
        {
            httpClient = new()
            {
                BaseAddress = new Uri("https://navixrecruitingcasestudy.blob.core.windows.net")
            };
        }

        public async Task<Vehicles> GetListOfAllVehiclesAsync()
        {
            var response = await httpClient.GetFromJsonAsync<Vehicles>("/manufacturers/vehicle-manufacturers.json");

            return response;
        }
    }
}
