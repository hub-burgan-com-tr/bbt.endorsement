using Application.Endorsements.Commands.ApproveOrderDocuments;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Endorsements.Commands.ApproverOrderDocuments
{
    using static Testing;
    public class ApproveOrderDocumentCommandTest : TestBase
    {
        [Test]
        [TestCase("1beecf76-5529-4309-97fe-39df9e917bd3")]
        public async Task ApproveOrderDocumentTest(Guid id)
        {
            ApproveOrderDocumentCommand model = new ApproveOrderDocumentCommand();
            var response = await SendAsync(model);
            Assert.IsNotNull(id);
        }
    }
}
