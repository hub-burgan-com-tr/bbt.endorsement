namespace Application.Documents.Commands.CreateDocumentCommands
{
    public class CreateDocumentCommandDto
    {
        public int Id { get; set; }
        public int ApprovalId { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public bool IsDocumentApproved { get; set; }
        public string DocumentApproved { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int FormId { get; set; }
        public string CitizenShipNumber { get; set; }
        public string NameAndSurname { get; set; }
    }


    
}
