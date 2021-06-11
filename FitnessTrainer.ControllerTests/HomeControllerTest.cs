using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FitnessTrainer.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FitnessTrainer.ControllerTests
{
    public class HomeControllerTest
    {
        private readonly ILogger<HomeController> _logger;

        public HomeControllerTest(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        [Fact]
        public async Task IndexPage()
        {
            HomeController controller = new HomeController(_logger);
            ViewResult view = controller.Index() as ViewResult;
            Assert.IsNotNull(view);
        }
    }
}
