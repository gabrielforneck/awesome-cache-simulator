using System.Collections;

namespace AWCSim.Core.TextReport.Sections;

public class TextReportSection : IEnumerable<string>
{
    protected List<string> Lines { get; }

    public TextReportSection()
    {
        Lines = [];
    }

    public TextReportSection AddLine(string line)
    {
        Lines.Add(line);
        return this;
    }

    public IEnumerator<string> GetEnumerator() => Lines.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
