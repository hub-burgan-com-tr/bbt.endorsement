using Aspose.Pdf;
using Dms.Integration.Infrastructure.Enums;
using Dms.Integration.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Dms.Integration.Infrastructure.DocumentGenerator;

public interface IHtmlToPdfConverter
{
    Task<byte[]> GeneratePdfContent(DocumentDefinitionType? key, string htmlContent);

    byte[] GenerateHtmlContent(byte[] pdfContent);
}

public class DefaultHtmlToPdfConverter : IHtmlToPdfConverter
{
    private readonly IPdfConverterService converter;
    private readonly ILogger logger;
    private static ConcurrentDictionary<string, byte[]> dictionary = new ConcurrentDictionary<string, byte[]>();

    public DefaultHtmlToPdfConverter(IPdfConverterService converter,
        ILoggerFactory loggerFactory)
    {
        this.converter = converter;
        this.logger = loggerFactory.CreateLogger<DefaultHtmlToPdfConverter>();
    }

    private static void StrategyOfSavingHtml(HtmlSaveOptions.HtmlPageMarkupSavingInfo htmlSavingInfo)
    {
        var reader = new System.IO.BinaryReader(htmlSavingInfo.ContentStream);
        byte[] htmlAsByte = reader.ReadBytes((int)htmlSavingInfo.ContentStream.Length);
        dictionary.AddOrUpdate(htmlSavingInfo.SupposedFileName, htmlAsByte, (key, value) => value);
    }

    public byte[] GenerateHtmlContent(byte[] pdfContent)
    {
        var license = new License();
        license.SetLicense("AsposeLicense/Aspose.Pdf.lic");
        var options = new HtmlSaveOptions();
        var fileName = $"{Guid.NewGuid()}.html";
        options.PartsEmbeddingMode = HtmlSaveOptions.PartsEmbeddingModes.EmbedAllIntoHtml;
        options.LettersPositioningMethod = HtmlSaveOptions.LettersPositioningMethods.UseEmUnitsAndCompensationOfRoundingErrorsInCss;
        options.RasterImagesSavingMode = HtmlSaveOptions.RasterImagesSavingModes.AsEmbeddedPartsOfPngPageBackground;
        options.DocumentType = HtmlDocumentType.Html5;
        options.RemoveEmptyAreasOnTopAndBottom = true;
        options.FixedLayout = true;

        options.CustomHtmlSavingStrategy = StrategyOfSavingHtml;


        using (var pdfStream = new MemoryStream(pdfContent))
        {
            var document = new Aspose.Pdf.Document(pdfStream);
            document.Save(fileName, options);
            dictionary.TryRemove(fileName, out byte[] result);
            return result;
        }
    }

    public async Task<byte[]> GeneratePdfContent(DocumentDefinitionType? key, string htmlContent)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        byte[] pdf = await converter.GeneratePdfContent(htmlContent);
       // logger.LogInformation(EventIdConstants.PdfConverterPerformanceEventId, "{DocumentDefinitionType} Pdf converted in ms {ElapsedMilliseconds} ", key?.ToString(), stopWatch.ElapsedMilliseconds);
        return pdf;
    }
}

