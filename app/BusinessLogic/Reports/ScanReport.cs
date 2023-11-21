using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data.Common;
using System.Drawing;
using System.Reflection;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace BusinessLogic.Reports
{
    public class ScanReport : IDocument
    {
        private const string criticalColor = "#FA2521";
        private const string highColor = "#D18224";
        private const string mediumColor = "#87859C";
        private const string lowColor = "#3D8361";

        private const string _highColor = "#FA2521";
        private const string _medColor = "#D18224";
        private const string _lowColor = "#87859C";
        private const string _okColor = "#3D8361";

        private List<ScanResult> scanResults;
        private List<SecurityCheck> securityChecks;

        public ScanReport(List<ScanResult> scanResults, List<SecurityCheck> securityChecks) 
        {
            this.scanResults = scanResults;
            this.securityChecks = securityChecks;
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

                    System.Console.WriteLine(System.Net.Dns.GetHostEntry("localhost").HostName);
                    column.Item().Text(text =>
                    {
                        text.Span("Machine Name: ").SemiBold();
                        text.Span($"{System.Net.Dns.GetHostEntry("localhost").HostName}");
                    });
                });

                var logoAsBytes = BitmapToByteArray(Resources.Resources.SecuriKeyLogo);
                row.ConstantItem(62).Height(125).Image(logoAsBytes);
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
            container.PaddingHorizontal(100).Table(table =>
            {
                // step 1 define number of columns
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(4);
                    columns.RelativeColumn(1);
                });

                // step 2 add column titles
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Severity");
                    header.Cell().Element(CellStyle).Text("Total");

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

                // only count the severities where possible action could be required
                var criticalCount = securityChecks.Count(c => c.Outcome == SecurityCheck.OutcomeTypes.ActionRecommended && c.Severity.Rating == Severities.CRITICAL);
                var highCount = securityChecks.Count(c => c.Outcome == SecurityCheck.OutcomeTypes.ActionRecommended && c.Severity.Rating == Severities.HIGH);
                var mediumCount = securityChecks.Count(c => c.Outcome == SecurityCheck.OutcomeTypes.ActionRecommended && c.Severity.Rating == Severities.MEDIUM);
                var lowCount = securityChecks.Count(c => c.Outcome == SecurityCheck.OutcomeTypes.ActionRecommended && c.Severity.Rating == Severities.LOW);
                
                table.Cell().Element(CellStyle).Text("Critical").FontColor(_highColor).Bold();
                table.Cell().Element(CellStyle).Text(criticalCount.ToString()).FontColor(Colors.Black).NormalWeight();

                table.Cell().Element(CellStyle).Text("High").FontColor(_medColor).Bold();
                table.Cell().Element(CellStyle).Text(highCount.ToString()).FontColor(Colors.Black).NormalWeight();

                table.Cell().Element(CellStyle).Text("Medium").FontColor(_lowColor).Bold();
                table.Cell().Element(CellStyle).Text(mediumCount.ToString()).FontColor(Colors.Black).NormalWeight();

                table.Cell().Element(CellStyle).Text("Low").FontColor(_okColor).Bold();
                table.Cell().Element(CellStyle).Text(lowCount.ToString()).FontColor(Colors.Black).NormalWeight();
                
            });
        }

        private void ComposeTableSummary(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().AlignRight().Text($"Total Scan Checks Performed: {securityChecks.Count.ToString()}").FontSize(14);
            });
        }

        void ComposeIntro(IContainer container)
        {
            container.PaddingHorizontal(10).Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
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

                column.Item().Text(text =>
                {

                    text
                        .Span($"SecuriKey recommends you review the following items to ensure your cyber hygiene.")
                        .Bold()
                        .FontColor(Colors.Black);

                });

                foreach (var result in securityChecks.Where(w => w.Outcome == SecurityCheck.OutcomeTypes.ActionRecommended))
                {
                    // determine severity color
                    string severityColor = string.Empty;
                    switch (result.Severity.Rating)
                    {
                        case Severities.CRITICAL:
                            severityColor = criticalColor;
                            break;
                        case Severities.HIGH:
                            severityColor = highColor;
                            break;
                        case Severities.MEDIUM:
                            severityColor = mediumColor;
                            break;
                        case Severities.LOW:
                            severityColor = lowColor;
                            break;
                        default:
                            severityColor = string.Empty;
                            break;
                    }

                    
                    column.Item().Text(text =>
                    {
                     
                        text
                            .Span($"{result.Severity.Rating.ToString().ToUpper()}")
                            .Bold()
                            .FontColor(severityColor);
                        text
                            .Span($"   {result.Name}   ")
                            .Bold()
                            .FontColor(Colors.Black);
                        text
                            .Line($"-  {result.Relevance}") // This ensures a new line starts after this
                            .NormalWeight()
                            .FontColor(Colors.BlueGrey.Medium);

                        // SecuriKey's recommendation
                        text
                            .Line($"SecuriKey's recommendation: {result.ActionRequiredMessage}   ") // Starts on a new line
                            .FontColor(Colors.Green.Darken4);

                        // In simpler terms
                        text
                            .Line("In simpler terms, " + result.SimpleAnalogy) // Starts on a new line
                            .NormalWeight()
                            .FontColor(Colors.Grey.Medium);

                    });

                    column.Spacing(20);


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
