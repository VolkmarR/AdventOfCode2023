namespace AdventOfCode.Day16;

public class Day16Tests
{
    private readonly ITestOutputHelper _output;
    public Day16Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day16Solver().ExecuteExample1("46");

    [Fact]
    public void Step2WithExample() => new Day16Solver().ExecuteExample2("51");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day16Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day16Solver().ExecutePuzzle2());
}

public class Day16Solver : SolverBase
{
    char[,] _map;
    HashSet<(int x, int y)> _energized;
    HashSet<(int x, int y, int d)> _splits;
    int _width;
    int _height;

    private Dictionary<(int dx, int dy), (int dx, int dy)> _rotateForwardSlash = new()
    {
        { (1, 0), (0, -1) },
        { (0, -1), (1, 0) },
        { (-1, 0), (0, 1) },
        { (0, 1), (-1, 0) },
    };

    private Dictionary<(int dx, int dy), (int dx, int dy)> _rotateBackwardSlash = new()
    {
        { (1, 0), (0, 1) },
        { (0, -1), (-1, 0) },
        { (-1, 0), (0, -1) },
        { (0, 1), (1, 0) },
    };


    protected override void Parse(List<string> data)
    {
        _energized = new();
        _splits = new();
        _map = new char[data[0].Length, data.Count];
        _width = _map.GetLength(0);
        _height = _map.GetLength(1);
        for (var y = 0; y < _height; y++)
        for (var x = 0; x < _width; x++)
            _map[x, y] = data[y][x];
    }


    void Beam(int x, int y, int dx, int dy)
    {
        while (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            var tile = _map[x, y];
            if (tile == '-' && dx == 0)
            {
                if (!_splits.Contains((x, y, dy)))
                {
                    _splits.Add((x, y, dy));
                    Beam(x, y, -1, 0);
                    Beam(x, y, 1, 0);
                }

                return;
            }

            if (tile == '|' && dy == 0)
            {
                if (!_splits.Contains((x, y, dx)))
                {
                    _splits.Add((x, y, dx));
                    Beam(x, y, 0, -1);
                    Beam(x, y, 0, 1);
                }

                return;
            }

            if (tile == '/')
                (dx, dy) = _rotateForwardSlash[(dx, dy)];
            else if (tile == '\\')
                (dx, dy) = _rotateBackwardSlash[(dx, dy)];

            _energized.Add((x, y));
            (x, y) = (x + dx, y + dy);
        }
    }

    int StartBeam(int x, int y, int dx, int dy, int max)
    {
        _splits.Clear();
        _energized.Clear();
        Beam(x, y, dx, dy);
        return Math.Max(_energized.Count, max);
    }

    protected override object Solve1() => StartBeam(0, 0, 1, 0, 0);

    protected override object Solve2()
    {
        var max = 0;
        for (int x = 0; x < _width; x++)
        {
            max = StartBeam(x, 0, 0, 1, max);
            max = StartBeam(x, _height - 1, 0, -1, max);
        }

        for (int y = 0; y < _width; y++)
        {
            max = StartBeam(0, y, 1, 0, max);
            max = StartBeam(_width - 1, y, -1, 0, max);
        }

        return max;
    }
}