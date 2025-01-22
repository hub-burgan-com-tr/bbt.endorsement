using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface ITemplateEngineService
    {
        Task<Dictionary<string, string>> HtmlRender(string templateName, string content);
        Task<Dictionary<string, string>> PdfRender(string templateName, string content);
    }
}
