using System.Collections.Generic;

namespace ImproveGameStructureDLC.UI;

public class StructureInfo
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string Author { get; set; }
    public string Date { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; } = [];
}
