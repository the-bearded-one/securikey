using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data.Common;
using System.Drawing;
using System.Reflection;

namespace BusinessLogic.Reports
{
    public class ScanReport : IDocument
    {
        private const string _highColor = "#FA2521";
        private const string _medColor = "#D18224";
        private const string _lowColor = "#87859C";
        private const string _okColor = "#3D8361";

        private List<ScanResult> scanResults;

        public ScanReport(List<ScanResult> scanResults) 
        {
            this.scanResults = scanResults;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }

        private void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"SecuriKey Scan Report").Style(titleStyle);

                    column.Item().Text(text =>
                    {
                        text.Span("Date: ").SemiBold();
                        text.Span($"{DateTime.Now.ToString("yyyy.MM.dd")}");
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Time: ").SemiBold();
                        text.Span($"{DateTime.Now.ToString("HH:mm:ss")}");
                    });
                });

                var logoAsBytes = BitmapToByteArray(Resources.Resources.SecuriKeyLogo);
                row.ConstantItem(50).Height(100).Image(logoAsBytes);
            });
        }

        void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Element(ComposeTable);
                column.Item().Element(ComposeTableSummary);
                column.Item().PaddingTop(25).Element(ComposeIntro);
                column.Item().PaddingTop(25).Element(ComposeScanResults);
                column.Item().PaddingTop(25).Element(ComposeEndReport);
            });
        }

        void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                // step 1 define number of columns
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                });

                // step 2 add column titles
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Severity");
                    header.Cell().Element(CellStyle).Text("Total");
                    header.Cell().Element(CellStyle).Text("Comment");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                // step 3 add column information
                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }

                var highNb = scanResults.Count(r => r.Severity == Severity.High);
                var medNb = scanResults.Count(r => r.Severity == Severity.Medium);
                var lowNb = scanResults.Count(r => r.Severity == Severity.Low);
                var okNb = scanResults.Count(r => r.Severity == Severity.Ok);

                table.Cell().Element(CellStyle).Text("High").FontColor(_highColor).Bold();
                table.Cell().Element(CellStyle).Text(highNb.ToString()).FontColor(Colors.Black).NormalWeight();
                table.Cell().Element(CellStyle).Text("Important to fix!").FontColor(Colors.Black).NormalWeight();

                table.Cell().Element(CellStyle).Text("Medium").FontColor(_medColor).Bold();
                table.Cell().Element(CellStyle).Text(medNb.ToString()).FontColor(Colors.Black).NormalWeight();
                table.Cell().Element(CellStyle).Text("Somewhat important to fix!").FontColor(Colors.Black).NormalWeight();

                table.Cell().Element(CellStyle).Text("Low").FontColor(_lowColor).Bold();
                table.Cell().Element(CellStyle).Text(lowNb.ToString()).FontColor(Colors.Black).NormalWeight();
                table.Cell().Element(CellStyle).Text("Fix if you have time!").FontColor(Colors.Black).NormalWeight();

                table.Cell().Element(CellStyle).Text("OK").FontColor(_okColor).Bold();
                table.Cell().Element(CellStyle).Text(okNb.ToString()).FontColor(Colors.Black).NormalWeight();
                table.Cell().Element(CellStyle).Text("No issues here!").FontColor(Colors.Black).NormalWeight();
            });
        }

        private void ComposeTableSummary(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().AlignRight().Text($"Total Scan Checks Performed: {scanResults.Count.ToString()}").FontSize(14);
            });
        }

        void ComposeIntro(IContainer container)
        {
            container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
            {
                column.Spacing(5);
                column.Item().Text("Disclaimer").FontSize(12).Bold();
                column.Item().Text("This report contains sensitive, privileged, and confidential information. Precautions should be taken to protect the confidentiality of the information in this document. Note that this assessment may not disclose all vulnerabilities present in the system. This report is a summary of the findings from a 'point-in-time' assessment. The use and interpretation of the software and/or report provided herein are solely at the risk of the user.");

                column.Item().Text("Purpose").FontSize(12).Bold();
                column.Item().Text("This report captures the results of a SecuriKey scan on a Windows computer and attempts to provide results of the security hygiene of the system and suggest methods to remediate the vulnerabilities where appropriate.");
            });
        }

        private void ComposeScanResults(IContainer container)
        {
            container.Column(column =>
            {
                foreach (var result in scanResults)
                {
                    /* create a header */

                    // determine severity color
                    var severityColor = _okColor;
                    switch (result.Severity)
                    {
                        case Severity.High:
                            severityColor = _highColor;
                            break;
                        case Severity.Medium:
                            severityColor = _medColor;
                            break;
                        case Severity.Low:
                            severityColor = _lowColor;
                            break;
                        default:
                            severityColor = _okColor;
                            break;
                    }

                    column.Item().Text(text =>
                    {
                        // Header
                        text
                            .Span($"{result.Severity.ToString().ToUpper()}")
                            .Bold()
                            .FontColor(severityColor);
                        text
                            .Span($"    {result.ScanType}    ")
                            .Bold()
                            .FontColor(Colors.Black);
                        text
                            .Line($"-    {result.ShortDescription}")
                            .NormalWeight()
                            .FontColor(Colors.BlueGrey.Medium);

                        // Details
                        text
                            .Line(result.DetailedDescription).NormalWeight().FontColor(Colors.Grey.Medium);
                    });
                }
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.CurrentPageNumber();
            });
        }

        private void ComposeEndReport(IContainer container)
        {
            container
                .AlignCenter()
                .AlignMiddle()
                .Text("END OF REPORT")
                .Bold()
                .FontSize(14);
        }

        // Helper method to convert a Bitmap to a byte array
        private static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
