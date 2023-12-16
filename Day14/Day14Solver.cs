namespace AdventOfCode.Day14;

public class Day14Tests
{
    private readonly ITestOutputHelper _output;
    public Day14Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day14Solver().ExecuteExample1("136");

    [Fact]
    public void Step2WithExample() => new Day14Solver().ExecuteExample2("64");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day14Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day14Solver().ExecutePuzzle2());
}

public class Day14Solver : SolverBase
{
    private int _width;
    private int _height;
    private char[,] _map1;
    private char[,] _map2;
    private int[] _firstFree;

    protected override void Parse(List<string> data)
    {
        _width = data[0].Length;
        _height = data.Count;
        _map1 = new char[_width, _height];
        for (int y = 0; y < _height; y++)
        for (int x = 0; x < _width; x++)
            _map1[x, y] = data[y][x];

        _map2 = new char[_width, _height];
        _firstFree = new int[_width];
    }

    void InitFree()
    {
        for (var i = 0; i < _width; i++)
            _firstFree[i] = -1;
    }

    void SetFirstFree(int index, int currentPos, char item)
    {
        if (item == '.' && _firstFree[index] == -1)
            _firstFree[index] = currentPos;
        else if (item != '.')
            _firstFree[index] = -1;
    }

    void TiltNorth(char[,] map, char[,] newMap)
    {
        InitFree();

        for (int y = 0; y < _height; y++)
        for (int x = 0; x < _width; x++)
        {
            var item = map[x, y];
            if (item == 'O' && _firstFree[x] > -1)
            {
                newMap[x, _firstFree[x]] = item;
                newMap[x, y] = '.';
                _firstFree[x]++;
            }
            else
            {
                newMap[x, y] = item;
                SetFirstFree(x, y, item);
            }
        }
    }

    void TiltWest(char[,] map, char[,] newMap)
    {
        InitFree();

        for (int x = 0; x < _width; x++)
        for (int y = 0; y < _height; y++)
        {
            var item = map[x, y];
            if (item == 'O' && _firstFree[y] > -1)
            {
                newMap[_firstFree[y], y] = item;
                newMap[x, y] = '.';
                _firstFree[y]++;
            }
            else
            {
                newMap[x, y] = item;
                SetFirstFree(y, x, item);
            }
        }
    }

    void TiltSouth(char[,] map, char[,] newMap)
    {
        InitFree();

        for (int y = _height - 1; y >= 0; y--)
        for (int x = 0; x < _width; x++)
        {
            var item = map[x, y];
            if (item == 'O' && _firstFree[x] > -1)
            {
                newMap[x, _firstFree[x]] = item;
                newMap[x, y] = '.';
                _firstFree[x]--;
            }
            else
            {
                newMap[x, y] = item;
                SetFirstFree(x, y, item);
            }
        }
    }

    void TiltEast(char[,] map, char[,] newMap)
    {
        InitFree();

        for (int x = _width - 1; x >= 0; x--)
        for (int y = 0; y < _height; y++)
        {
            var item = map[x, y];
            if (item == 'O' && _firstFree[y] > -1)
            {
                newMap[_firstFree[y], y] = item;
                newMap[x, y] = '.';
                _firstFree[y]--;
            }
            else
            {
                newMap[x, y] = item;
                SetFirstFree(y, x, item);
            }
        }
    }

    void TiltFull()
    {
        TiltNorth(_map1, _map2);
        TiltWest(_map2, _map1);
        TiltSouth(_map1, _map2);
        TiltEast(_map2, _map1);
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

    protected override object Solve1()
    {
        TiltNorth(_map1, _map2);
        return TotalLoad(_map2);
    }

    protected override object Solve2()
    {
        for (int i = 0; i < 1_000_000_000; i++)
            TiltFull();

        return TotalLoad(_map1);
    }
}