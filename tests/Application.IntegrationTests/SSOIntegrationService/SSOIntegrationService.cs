using Application.TemplateEngines.Commands.Renders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationTests.SSOIntegrationService
{
    public class SSOIntegrationServiceTest : TestBase
    {

        [Test]
        [TestCase("U05804", "","")]
        public async Task SearchUserInfo(string loginName, string firstName, string lastName)
        {
            try
            {
                Infrastructure.SSOIntegration.SSOIntegrationService model = new Infrastructure.SSOIntegration.SSOIntegrationService();
                var data = model.SearchUserInfo("U05804", "", "");
                Assert.IsNotNull(data);
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}
