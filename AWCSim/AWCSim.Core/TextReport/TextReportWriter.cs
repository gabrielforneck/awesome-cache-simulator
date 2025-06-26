using AWCSim.Core.TextReport.Sections;

namespace AWCSim.Core.TextReport;

public class TextReportWriter
{
    protected string Title { get; }
    protected List<TextReportSection> Sections { get; }
    protected TextReportSection CurrentSection { get; set; }

    protected const int MinimunLineLength = 10;
    protected const char DefaultHorizontalSeparator = '-';
    protected const char DefaultVerticalSeparator = '|';

    public TextReportWriter(string title)
    {
        Title = title;
        CurrentSection = new TextReportSection();
        Sections = [CurrentSection];
    }

    public TextReportWriter AddSection()
    {
        CurrentSection = new();
        Sections.Add(CurrentSection);
        return this;
    }

    public TextReportWriter AddLine(string text)
    {
        CurrentSection.AddLine(text);
        return this;
    }

    public void Write(string path)
    {
        var biggestLineLength = FindBiggestLineLength();

        Write(path, biggestLineLength);
    }

    protected void Write(string path, int lineLength)
    {
        using var stream = File.OpenWrite(path);
        using var writer = new StreamWriter(stream);

        var lineSeparator = CreateLineSeparator(lineLength);
        writer.WriteLine(lineSeparator);
        writer.WriteLine(FormatLine(Title, lineLength));
        writer.WriteLine(lineSeparator);

        foreach (var section in Sections)
        {
            foreach (var line in section)
                writer.WriteLine(FormatLine(line, lineLength));

            writer.WriteLine(lineSeparator);
        }
    }

    protected static string CreateLineSeparator(int lineLength)
    {
        var line = " + ";

        line += new string(DefaultHorizontalSeparator, lineLength);

        return line + " + ";
    }

    protected static string FormatLine(string text, int lineLength)
    {
        if (text.Length < lineLength)
            text += new string(' ', lineLength - text.Length);

        return $" {DefaultHorizontalSeparator} {text} {DefaultHorizontalSeparator} ";
    }

    protected int FindBiggestLineLength()
    {
        var titleLength = Title.Length;
        var biggestLineLength = Sections.SelectMany(s => s).Select(l => l.Length).Max();

        if (titleLength < MinimunLineLength && biggestLineLength < MinimunLineLength)
            return MinimunLineLength;

        if (titleLength > biggestLineLength)
            return titleLength;

        return biggestLineLength;
    }
}
