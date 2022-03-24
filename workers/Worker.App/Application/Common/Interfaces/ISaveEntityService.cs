namespace Worker.App.Application.Common.Interfaces
{
    public interface ISaveEntityService
    {
        Task<string> GetCustomerAsync(string citizenshipNumber);
        Task<string> CustomerSaveAsync(string citizenshipNumber, string firstName, string lastName);
        Task<string> GetApproverAsync(string citizenshipNumber);
        Task<string> ApproverSaveAsync(string citizenshipNumber, string firstName, string lastName);
    }
}
