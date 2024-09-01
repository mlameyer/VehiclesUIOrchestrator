using Microsoft.VisualStudio.TestTools.UnitTesting;
using VehiclesUIOrchestrator.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehiclesUIOrchestrator.Repositories.Tests
{
    [TestClass]
    public class NavixCaseStudyRepositoryTests
    {
        [TestMethod()]
        public async Task GetListOfAllVehiclesAsyncTest()
        {
            NavixCaseStudyRepository repo = new NavixCaseStudyRepository();
            var response = await repo.GetListOfAllVehiclesAsync();
            Assert.IsNotNull(response);
        }
    }
}