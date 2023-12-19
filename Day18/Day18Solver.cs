namespace AdventOfCode.Day18;

public class Day18Tests
{
    private readonly ITestOutputHelper _output;
    public Day18Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day18Solver().ExecuteExample1("62");

    [Fact]
    public void Step2WithExample() => new Day18Solver().ExecuteExample2("??");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day18Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day18Solver().ExecutePuzzle2());
}

public class Day18Solver : SolverBase
{
    record Step(char Direction, int Length, string Color);

    List<Step> _data;
    private Dictionary<(int X, int Y), string> _map;

    protected override void Parse(List<string> data)
    {
        Step Parse(string line)
        {
            var parts = line.Split(new[] { ' ', '(', ')', }, StringSplitOptions.RemoveEmptyEntries);
            return new(parts[0][0], parts[1].ToInt(), parts[2]);
        }

        _data = data.Select(Parse).ToList();
        _map = new();
    }

    void Dig()
    {
        var (x, y) = (0, 0);
        _map[(0, 0)] = "";
        foreach (var step in _data)
        {
            int dx = step.Direction == 'L' ? -1 : step.Direction == 'R' ? 1 : 0;
            int dy = step.Direction == 'U' ? -1 : step.Direction == 'D' ? 1 : 0;
            for (int i = 0; i < step.Length; i++)
            {
                x += dx;
                y += dy;
                _map[(x, y)] = step.Color;
            }
        }
    }

    void Dump()
    {
        var minX = _map.Keys.Min(q => q.X);
        var minY = _map.Keys.Min(q => q.Y);
        var maxX = _map.Keys.Max(q => q.X);
        var maxY = _map.Keys.Max(q => q.Y);
        var point = "S";

        var sb = new StringBuilder();
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                sb.Append(_map.ContainsKey((x, y)) ? point : ".");
                point = "#";
            }

            sb.AppendLine();
        }

        File.WriteAllText(Path.Combine(DayDirectory, "map.txt"), sb.ToString());
    }

    void StartFill()
    {
        var minX = _map.Keys.Min(q => q.X);
        var minY = _map.Keys.Min(q => q.Y);
        var maxX = _map.Keys.Max(q => q.X);
        var maxY = _map.Keys.Max(q => q.Y);
        for (int y = minY; y <= maxY; y++)
        for (int x = minX; x <= maxX; x++)
        {
            if (_map.ContainsKey((x, y)))
                continue;

            var found = false;
            for (int xx = x; xx <= maxX && !found; xx++)
                found = _map.ContainsKey((xx, y));
            if (!found)
                continue;

            found = false;
            for (int xx = x; xx >= minX && !found; xx--)
                found = _map.ContainsKey((xx, y));
            if (!found)
                continue;

            found = false;
            for (int yy = y; yy <= maxY && !found; yy++)
                found = _map.ContainsKey((x, yy));
            if (!found)
                continue;

            found = false;
            for (int yy = y; yy >= minY && !found; yy--)
                found = _map.ContainsKey((x, yy));
            if (!found)
                continue;

            Fill(x, y);
            return;
        }
    }

    void Fill(int x, int y)
    {
        if (_map.ContainsKey((x, y)))
            return;

        var minX = x;
        while (!_map.ContainsKey((minX, y)))
            minX--;

        var maxX = minX + 1;
        while (!_map.ContainsKey((maxX, y)))
        {
            _map[(maxX, y)] = "";
            maxX++;
        }

        var minY = y;
        while (!_map.ContainsKey((x, minY)))
            minY--;

        var maxY = minY + 1;
        while (!_map.ContainsKey((x, maxY)))
        {
            _map[(x, maxY)] = "";
            maxY++;
        }

        for (int xx = minX + 1; xx < maxX; xx++)
        {
            if (!_map.ContainsKey((xx, y - 1)))
                Fill(xx, y-1);
            if (!_map.ContainsKey((xx, y + 1)))
                Fill(xx, y+1);
        }
  
        for (int yy = minY + 1; yy < maxY; yy++)
        {
            if (!_map.ContainsKey((x - 1, yy)))
                Fill(x - 1, yy);
            if (!_map.ContainsKey((x + 1, yy)))
                Fill(x +1, yy);
        }
    }


    protected override object Solve1()
    {
        Dig();
        StartFill();
        Dump();

        return _map.Count();
    }

    protected override object Solve2()
    {
        throw new Exception("Solver error");
    }
}