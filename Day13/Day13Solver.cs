namespace AdventOfCode.Day13;

public class Day13Tests
{
    private readonly ITestOutputHelper _output;
    public Day13Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day13Solver().ExecuteExample1("405");

    [Fact]
    public void Step2WithExample() => new Day13Solver().ExecuteExample2("400");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day13Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day13Solver().ExecutePuzzle2());
}

public class Day13Solver : SolverBase
{
    private class Map
    {
        private char[,] _map;
        private int _width;
        private int _height;

        public Map(char[,] map)
        {
            _map = map;
            _width = _map.GetLength(0);
            _height = _map.GetLength(1);
        }

        bool IsReflectionVertical(int xLeft, int xRight)
        {
            while (xLeft >= 0 && xRight < _width)
            {
                for (var y = 0; y < _height; y++)
                {
                    if (_map[xLeft, y] != _map[xRight, y])
                        return false;
                }

                xLeft--;
                xRight++;
            }

            return true;
        }

        bool IsReflectionHorizontal(int yTop, int yBottom)
        {
            while (yTop >= 0 && yBottom < _height)
            {
                for (var x = 0; x < _width; x++)
                {
                    if (_map[x, yTop] != _map[x, yBottom])
                        return false;
                }

                yTop--;
                yBottom++;
            }

            return true;
        }

        int ReflectionHorizontalRow(int skipRow = -1)
        {
            for (int y = 1; y < _height; y++)
                if (y != skipRow && IsReflectionHorizontal(y - 1, y))
                    return y;

            return -1;
        }

        int ReflectionVerticalCol(int skipCol = -1)
        {
            for (var x = 1; x < _width; x++)
                if (x != skipCol && IsReflectionVertical(x - 1, x))
                    return x;

            return -1;
        }

        public int ReflectionValue(int skipCol = -1, int skipRow = -1)
        {
            var result = ReflectionVerticalCol(skipCol);
            if (result == -1)
            {
                result = ReflectionHorizontalRow(skipRow);
                if (result > -1)
                    result *= 100;
            }

            return result;
        }

        public int ReflectionValueSmudge()
        {
            var standardCol = ReflectionVerticalCol();
            var standardRow = ReflectionHorizontalRow();
            for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
            {
                void Flip()
                {
                    _map[x, y] = _map[x, y] == '#' ? '.' : '#';
                }

                Flip();
                var newValue = ReflectionValue(standardCol, standardRow);
                Flip();
                if (newValue > -1)
                    return newValue;
            }

            return ReflectionValue();
        }
    }

    List<Map> _data;

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
            _data.Add(new Map(map));
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

    protected override object Solve1()
        => _data.Sum(q => q.ReflectionValue());

    protected override object Solve2()
        => _data.Sum(q => q.ReflectionValueSmudge());
}