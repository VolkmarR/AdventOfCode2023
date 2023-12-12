namespace AdventOfCode.Day11;

public class Day11Tests
{
    private readonly ITestOutputHelper _output;
    public Day11Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day11Solver().ExecuteExample1("374");

    [Fact]
    public void Step2WithExample() => new Day11Solver().ExecuteExample2("8410");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day11Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day11Solver().ExecutePuzzle2());
}

public class Day11Solver : SolverBase
{
    private record Galaxy(long x, long y);

    List<Galaxy> _data;
    private int _width;
    private int _height;

    protected override void Parse(List<string> data)
    {
        _data = new();
        _width = data[0].Length;
        _height = data.Count;
        for (int y = 0; y < _height; y++)
        for (int x = 0; x < _width; x++)
        {
            if (data[y][x] == '#')
                _data.Add(new(x, y));
        }
    }

    private List<Galaxy> Expand(long expansion = 1)
    {
        var xList = _data.GroupBy(q => q.x).ToDictionary(q => q.Key, q => q.ToList());
        var xExpanded = new List<Galaxy>();
        var xExpand = 0L;
        for (long x = 0; x < _height; x++)
        {
            if (xList.TryGetValue(x, out var items))
            {
                foreach (var item in items)
                    xExpanded.Add(new(item.x + xExpand, item.y));
            }
            else
                xExpand += expansion;
        }

        var yList = xExpanded.GroupBy(q => q.y).ToDictionary(q => q.Key, q => q.ToList());
        var yExpanded = new List<Galaxy>();
        var yExpand = 0L;
        for (long y = 0; y < _height; y++)
        {
            if (yList.TryGetValue(y, out var items))
            {
                foreach (var item in items)
                    yExpanded.Add(new(item.x, item.y + yExpand));
            }
            else
                yExpand += expansion;
        }

        return yExpanded;
    }

    List<(Galaxy item1, Galaxy item2)> GetPairs(List<Galaxy> items)
    {
        var result = new List<(Galaxy item1, Galaxy item2)>();
        for (int i = 0; i < items.Count - 1; i++)
        {
            var g1 = items[i];
            for (int j = i + 1; j < items.Count; j++)
            {
                var g2 = items[j];
                result.Add((g1, g2));
            }
        }

        return result;
    }

    List<long> GetDistance(List<(Galaxy item1, Galaxy item2)> pairs)
    {
        var result = new List<long>();
        foreach (var ((x1, y1), (x2, y2)) in pairs)
            result.Add(Math.Abs(x2 - x1) + Math.Abs(y2 - y1));
        return result;
    }

    protected override object Solve1()
    {
        var expanded = Expand();
        var pairs = GetPairs(expanded);
        return GetDistance(pairs).Sum();
    }

    protected override object Solve2()
    {
        var expanded = Expand(1000000 - 1);
        var pairs = GetPairs(expanded);
        return GetDistance(pairs).Sum();
    }
}