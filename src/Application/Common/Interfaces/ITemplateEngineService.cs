using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface ITemplateEngineService
    {
        Task<Response<string>> HtmlRender(string templateName, string content);
        Task<Response<string>> PdfRender(string templateName, string content);
    }
}
