namespace AdventOfCode.Day14;

public class Day14Tests
{
    private readonly ITestOutputHelper _output;
    public Day14Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day14Solver().ExecuteExample1("136");

    [Fact]
    public void Step2WithExample() => new Day14Solver().ExecuteExample2("??");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day14Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day14Solver().ExecutePuzzle2());
}

public class Day14Solver : SolverBase
{
    char[,] _data;

    protected override void Parse(List<string> data)
    {
        _data = new char[data[0].Length, data.Count];
        for (int y = 0; y < data.Count; y++)
        for (int x = 0; x < data[y].Length; x++)
            _data[x, y] = data[y][x];
    }

    char[,] Tilt(char[,] map)
    {
        var newMap = new char[map.GetLength(0), map.GetLength(1)];
        var firstFreeY = new int[map.GetLength(0)];
        for (int x = 0; x < firstFreeY.Length; x++)
            firstFreeY[x] = -1;

        for (int y = 0; y < map.GetLength(1); y++)
        for (int x = 0; x < map.GetLength(0); x++)
        {
            var item = map[x, y];
            if (item == 'O' && firstFreeY[x] > -1)
            {
                newMap[x, firstFreeY[x]] = item;
                newMap[x, y] = '.';
                firstFreeY[x]++;
            }
            else
            {
                newMap[x, y] = item;
                if (item == '.' && firstFreeY[x] == -1)
                    firstFreeY[x] = y;
                else if (item != '.')
                    firstFreeY[x] = -1;
            }
        }

        return newMap;
    }

    void Dump(char[,] map)
    {
        var sb = new StringBuilder();
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
                sb.Append(map[x, y]);
            sb.AppendLine();
        }

        File.WriteAllText(Path.Combine(DayDirectory, "map.txt"), sb.ToString());
    }

    int TotalLoad(char[,] map)
    {
        var result = 0;
        var height = map.GetLength(1);
        for (int y = 0; y < map.GetLength(1); y++)
        for (int x = 0; x < map.GetLength(0); x++)
        {
            if (map[x, y] == 'O')
                result += height - y;
        }

        return result;
    }

    protected override object Solve1() => TotalLoad(Tilt(_data));

    protected override object Solve2()
    {
        throw new Exception("Solver error");
    }
}