namespace AdventOfCode.Day13;

public class Day13Tests
{
    private readonly ITestOutputHelper _output;
    public Day13Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day13Solver().ExecuteExample1("405");

    [Fact]
    public void Step2WithExample() => new Day13Solver().ExecuteExample2("??");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day13Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day13Solver().ExecutePuzzle2());
}

public class Day13Solver : SolverBase
{
    List<char[,]> _data;

    protected override void Parse(List<string> data)
    {
        _data = new();
        var mapLines = new List<string>();

        void AddMap()
        {
            var map = new char[mapLines[0].Length, mapLines.Count];
            for (var y = 0; y < mapLines.Count; y++)
            for (var x = 0; x < mapLines[y].Length; x++)
                map[x, y] = mapLines[y][x];
            _data.Add(map);
            mapLines.Clear();
        }

        foreach (var line in data)
        {
            if (line == "")
                AddMap();
            else
                mapLines.Add(line);
        }

        AddMap();
    }

    bool IsReflectionVertical(char[,] map, int width, int height, int xLeft, int xRight)
    {
        while (xLeft >= 0 && xRight < width)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[xLeft, y] != map[xRight, y])
                    return false;
            }

            xLeft--;
            xRight++;
        }

        return true;
    }

    int ReflectionVerticalCol(char[,] map)
    {
        var width = map.GetLength(0);
        var height = map.GetLength(1);

        for (var x = 1; x < width; x++)
            if (IsReflectionVertical(map, width, height, x - 1, x))
                return x;

        return 0;
    }

    bool IsReflectionHorizontal(char[,] map, int width, int height, int yTop, int yBottom)
    {
        while (yTop >= 0 && yBottom < height)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, yTop] != map[x, yBottom])
                    return false;
            }

            yTop--;
            yBottom++;
        }

        return true;
    }

    int ReflectionHorizontalRow(char[,] map)
    {
        var width = map.GetLength(0);
        var height = map.GetLength(1);

        for (int y = 1; y < height; y++)
            if (IsReflectionHorizontal(map, width, height, y - 1, y))
                return y;

        return 0;
    }


    protected override object Solve1()
        => _data.Sum(q => ReflectionVerticalCol(q) + ReflectionHorizontalRow(q) * 100);

    protected override object Solve2()
    {
        throw new Exception("Solver error");
    }
}