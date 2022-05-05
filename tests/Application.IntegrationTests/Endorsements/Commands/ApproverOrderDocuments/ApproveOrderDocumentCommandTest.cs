using Application.Endorsements.Commands.ApproveOrderDocuments;
using Application.IntegrationTests.Services;
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
        [TestCaseSource(typeof(EndorsementService), nameof(EndorsementService.ApproveOrderDocumentTestData))]

        public async Task ApproveOrderDocumentTest(ApproveOrderDocumentCommand request)
        {
            var response = await SendAsync(request);
            Assert.IsNotNull(response.Data);
        }
    }
}



