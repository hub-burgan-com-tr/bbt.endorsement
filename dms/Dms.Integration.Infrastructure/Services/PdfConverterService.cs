using Aspose.Pdf;
using Aspose.Pdf.Devices;
using PugPdf.Core;
using System.Drawing;

namespace Dms.Integration.Infrastructure.Services;

public interface IPdfConverterService
{
    Task<byte[]> GeneratePdfContent(string htmlContent);

    byte[] ImagesToPdfContent(IEnumerable<byte[]> contents);

    List<byte[]> PdfToImageContents(byte[] pdfContent);
}

public class PdfConverterService : IPdfConverterService
{
    public async Task<byte[]> GeneratePdfContent(string htmlContent)
    {
        var renderer = new HtmlToPdf();

        renderer.PrintOptions.Title = "My title";

        var pdf = await renderer.RenderHtmlAsPdfAsync(htmlContent);
        return pdf.BinaryData;

        //var license = new License();
        //license.SetLicense("AsposeLicense/Aspose.Pdf.lic");
        //var bytes = Encoding.UTF8.GetBytes(htmlContent);
        //var options = new HtmlLoadOptions
        //{
        //    PageInfo = new PageInfo()
        //    {
        //        Margin = new MarginInfo
        //        {
        //            Bottom = 20,
        //            Left = 20,
        //            Right = 20,
        //            Top = 20
        //        }
        //    }
        //};
        //using (var ms = new MemoryStream(bytes))
        //using (var document = new Document(ms, options))
        //using (var memoryStream = new MemoryStream())
        //{
        //    Aspose.Pdf.Optimization.OptimizationOptions optimizationOptions = new Aspose.Pdf.Optimization.OptimizationOptions
        //    {
        //        RemoveUnusedObjects = true,//This may happen, for example, when a page is removed from the document page tree but the page object itself isn't removed
        //        RemoveUnusedStreams = true,// Page contents are analyzed in order to determine if a resource stream is used or not. Unused streams are removed.
        //        AllowReusePageContent = true,//If this property is set to true, the page content will be reused when optimizing the document for identical pages.
        //    };
        //    optimizationOptions.ImageCompressionOptions.CompressImages = true;
        //    optimizationOptions.ImageCompressionOptions.Version = Aspose.Pdf.Optimization.ImageCompressionVersion.Fast;

        //    document.OptimizeResources(optimizationOptions);
        //    document.Save(memoryStream, SaveFormat.Pdf);
        //    return memoryStream.ToArray();
        //}

    }
    public byte[] ImagesToPdfContent(IEnumerable<byte[]> contents)
    {
        var license = new License();
        license.SetLicense("AsposeLicense/Aspose.Pdf.lic");
        using (var doc = new Aspose.Pdf.Document())
        using (var outStream = new MemoryStream())
        {
            foreach (var cont in contents)
            {
                var page = doc.Pages.Add();
                var mystream = new MemoryStream(cont);
                // Create an image object
                var image1 = new Aspose.Pdf.Image();
                // Instantiate BitMap object with loaded image stream
                using (var b = new Bitmap(mystream))
                {
                    // Set margins so image will fit, etc.
                    page.PageInfo.Margin.Bottom = 0;
                    page.PageInfo.Margin.Top = 0;
                    page.PageInfo.Margin.Left = 0;
                    page.PageInfo.Margin.Right = 0;
                    page.CropBox = new Aspose.Pdf.Rectangle(0, 0, b.Width, b.Height);
                    page.SetPageSize(b.Width, b.Height);
                }

                // Add the image into paragraphs collection of the section
                page.Paragraphs.Add(image1);

                // Set the image file stream
                image1.ImageStream = mystream;
            }
            doc.Save(outStream, SaveFormat.Pdf);
            byte[] docBytes = outStream.ToArray();
            return docBytes;
        }
    }

    public List<byte[]> PdfToImageContents(byte[] pdfContent)
    {
        var license = new License();
        license.SetLicense("AsposeLicense/Aspose.Pdf.lic");
        using (var ms = new MemoryStream(pdfContent))
        using (var pdfDocument = new Aspose.Pdf.Document(ms))
        {
            var imageList = new List<byte[]>();
            foreach (var page in pdfDocument.Pages)
            {

                using (var ms2 = new MemoryStream())
                {
                    var jpg = new JpegDevice();
                    jpg.Process(page, ms2);
                    imageList.Add(ms2.ToArray());
                }
            }
            return imageList;
        }
    }


}