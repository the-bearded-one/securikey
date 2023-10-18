using BusinessLogic;
using System.Text;

public class ScanResult
{
    public string ScanType { get; set; }
    public string ShortDescription { get; set; }
    public string DetailedDescription { get; set; }
    public Severity Severity { get; set; }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Scan Type: {ScanType}");
        sb.AppendLine($"Short Description: {ShortDescription}");
        sb.AppendLine($"Detailed Description: {SplitStringIntoLines(DetailedDescription, 16)}"); // detailed description can be too long for pdf width, so split it up every 16 words
        sb.AppendLine($"Severity: {Severity.ToString()}");
        return sb.ToString();
    }
    private string SplitStringIntoLines(string input, int wordsPerLine)
    {
        string[] words = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        int startIndex = 0;
        int endIndex = Math.Min(wordsPerLine, words.Length);
        string result = "";

        while (startIndex < words.Length)
        {
            string line = string.Join(" ", words[startIndex..endIndex]);
            result += line + Environment.NewLine;
            startIndex = endIndex;
            endIndex = Math.Min(endIndex + wordsPerLine, words.Length);
        }

        return result;
    }
}