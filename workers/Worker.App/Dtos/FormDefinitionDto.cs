namespace Worker.App.Dtos
{
    public class FormDefinitionDto
    {
        public int ExpireInMinutes { get; set; }
        public string RetryFrequence { get; set; }
        public int MaxRetryCount { get; set; }
        public IEnumerable<FormDefinitionActionDto> Actions { get; set; }
    }

    public class FormDefinitionActionDto
    {
        public string Title { get; set; }
        public string State { get; set; }
        public bool IsDefault { get; set; }
    }
}
