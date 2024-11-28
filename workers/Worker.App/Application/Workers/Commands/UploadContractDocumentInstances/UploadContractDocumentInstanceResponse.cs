using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Worker.App.Application.Workers.Commands.UploadContractDocumentInstances
{
    public class UploadContractDocumentInstanceResponse
    {
        public Guid ContractInstanceId { get; set; }
        public string ContractCode { get; set; }
        public string Language { get; set; }
    }
}