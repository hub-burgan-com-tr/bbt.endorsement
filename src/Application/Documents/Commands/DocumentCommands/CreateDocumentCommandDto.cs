using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Documents.Commands.DocumentCommands
{
    public class CreateDocumentCommandDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public string DocumentApproved { get; set; }
    }
}
