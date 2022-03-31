namespace Worker.App.Dtos
{
    public class FormDefinitionDto
    {
        public int ExpireInMinutes { get; set; }
        public int RetryFrequence { get; set; }
        public int MaxRetryCount { get; set; }
        public string Type { get; set; }
        public IEnumerable<FormDefinitionActionDto> Actions { get; set; }
    }

    public class FormDefinitionActionDto
    {
        public string Title { get; set; }
        public string State { get; set; }
        public int Choice { get; set; }
    }
}
