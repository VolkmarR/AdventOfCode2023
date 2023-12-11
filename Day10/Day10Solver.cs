using StringBuilder = System.Text.StringBuilder;

namespace AdventOfCode.Day10;

public class Day10Tests
{
    private readonly ITestOutputHelper _output;
    public Day10Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day10Solver().ExecuteExample1("8");

    [Fact]
    public void Step2WithExample() => new Day10Solver().ExecuteExample2("??");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day10Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day10Solver().ExecutePuzzle2());
}

public class Day10Solver : SolverBase
{
    private char[,] _map;
    private (int X, int Y) _start;
    private readonly (int X, int Y) _invalid = (-1, -1);

    private (int x, int y) GetNextPosition((int x, int y) last, (int x, int y) current)
    {
        var (x, y) = current;
        if (x < 0 || x >= _map.GetLength(0) ||
            y < 0 || y >= _map.GetLength(1))
            return _invalid;

        (int x, int y) diff = (x - last.x, y - last.y);

        return _map[x, y] switch
        {
            '|' => (x, y + diff.y),
            '-' => (x + diff.x, y),
            'L' when diff.x == -1 => (x, y - 1),
            'L' when diff.y == 1 => (x + 1, y),
            'J' when diff.x == 1 => (x, y - 1),
            'J' when diff.y == 1 => (x - 1, y),
            '7' when diff.x == 1 => (x, y + 1),
            '7' when diff.y == -1 => (x - 1, y),
            'F' when diff.x == -1 => (x, y + 1),
            'F' when diff.y == -1 => (x + 1, y),
            _ => _invalid,
        };
    }

    private List<(int x, int y)> GetLoopPositions(int xDirection, int yDirection)
    {
        var result = new List<(int x, int y)>() { _start };
        var last = _start;
        var current = (x: _start.X + xDirection, y: _start.Y + yDirection);
        while (current != _start && current != _invalid)
        {
            result.Add(current);
            current = GetNextPosition(last, current);
            last = result.Last();
        }

        return current == _start ? result : null;
    }

    private List<(int x, int y)> GetLoopPositions()
    {
        var possibleNextSteps = new List<(int x, int y)>
        {
            (0, -1),
            (1, 0),
            (0, 1),
            (-1, 0)
        };

        foreach (var nextStart in possibleNextSteps)
        {
            var result = GetLoopPositions(nextStart.x, nextStart.y);
            if (result != null)
                return result;
        }

        return null;
    }


    protected override void Parse(List<string> data)
    {
        _map = new char[data[0].Length, data.Count];
        for (var y = 0; y < data.Count; y++)
        {
            for (var x = 0; x < data[y].Length; x++)
            {
                _map[x, y] = data[y][x];
                if (_map[x, y] == 'S')
                    _start = (x, y);
            }
        }
    }

    protected override object Solve1()
    {
        var loop = GetLoopPositions();
        if (loop != null)
            return loop.Count / 2;
        return 0;
    }

    protected override object Solve2()
    {
        var loop = GetLoopPositions();
        if (loop == null)
            return "";

        var map = _map.Clone() as char[,];

        void DumpMap(string fileName)
        {
            var sb = new StringBuilder();
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                    if (map[x, y] == '*' || map[x, y] == '.'|| map[x, y] == 'S')
                        sb.Append(map[x, y]);
                    else
                        sb.Append(' ');
                sb.AppendLine();
            }

            File.WriteAllText(Path.Combine(DayDirectory, fileName), sb.ToString());
        }

        foreach (var (x, y) in loop.Skip(1))
            map[x, y] = '*';

        DumpMap("map1.txt");

        return null;
    }
}