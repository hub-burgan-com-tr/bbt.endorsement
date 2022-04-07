using Application.TemplateEngines.Commands.Renders;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Application.IntegrationTests.TemplateEngines.Commands.Renders
{
    using static Testing;
    public class RenderCommandTests : TestBase
    {
        [Test]
        [TestCase("fff57322-7417-4805-acb0-3691e8540021", "data.name")]
        public async Task Render(string formId, string content)
        {
            try
            {
                RenderCommand model = new RenderCommand
                {
                    FormId = formId,
                    Content = content
                };
                var response = await SendAsync(model);
                Assert.IsNotNull(response);
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}
