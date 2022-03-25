namespace Application.Endorsements.Commands.NewOrders
{
    public class StartFormRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FormId { get; set; }
        public string Content { get; set; }
        public OrderReference Reference { get; set; }
        public PersonSummary Person { get; set; }
    }

    public class FormDefinitionClass
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string[] Tags { get; set; }
        /// <summary>
        /// If form data is used for rendering a document, render data with dedicated template in template engine.
        /// </summary>
        public string TemplateName { get; set; }
    }
}
