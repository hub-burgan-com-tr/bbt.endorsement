namespace Application.Endorsements.Queries.GetSearchPersonSummary
{
    public class GetSearchPersonSummaryResponse
    {
        public IEnumerable<GetSearchPersonSummaryDto> Persons { get; set; }
        public GetSearchPersonSummaryDto Person { get; set; }
    }
}
