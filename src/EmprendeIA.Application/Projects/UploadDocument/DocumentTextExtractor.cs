using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using UglyToad.PdfPig;

namespace EmprendeIA.Application.Projects.UploadDocument;

public static class DocumentTextExtractor
{
    public static string ExtractText(Stream fileStream, string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();

        return ext switch
        {
            ".txt" => ExtractTxt(fileStream),
            ".docx" => ExtractDocx(fileStream),
            ".pdf" => ExtractPdf(fileStream),
            _ => throw new NotSupportedException($"Formato '{ext}' no soportado. Use .txt, .docx o .pdf.")
        };
    }

    private static string ExtractTxt(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
        return reader.ReadToEnd();
    }

    private static string ExtractDocx(Stream stream)
    {
        // DOCX is a ZIP containing word/document.xml
        using var zip = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true);
        var entry = zip.GetEntry("word/document.xml")
            ?? throw new InvalidOperationException("El archivo DOCX no contiene word/document.xml");

        using var entryStream = entry.Open();
        var doc = XDocument.Load(entryStream);

        XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
        var sb = new StringBuilder();
        foreach (var text in doc.Descendants(w + "t"))
        {
            sb.Append(text.Value);
            sb.Append(' ');
        }
        return sb.ToString().Trim();
    }

    private static string ExtractPdf(Stream stream)
    {
        // Copy to MemoryStream because PdfPig needs seekable stream
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        ms.Position = 0;

        using var pdf = PdfDocument.Open(ms);
        var sb = new StringBuilder();
        foreach (var page in pdf.GetPages())
        {
            sb.AppendLine(page.Text);
        }
        return sb.ToString().Trim();
    }
}
