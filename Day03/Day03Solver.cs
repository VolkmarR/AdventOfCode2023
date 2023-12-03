namespace AdventOfCode.Day03;

public class Day03Tests
{
    private readonly ITestOutputHelper _output;
    public Day03Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day03Solver().ExecuteExample1("4361");

    [Fact]
    public void Step2WithExample() => new Day03Solver().ExecuteExample2("467835");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day03Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day03Solver().ExecutePuzzle2());
}

public class Day03Solver : SolverBase
{
    private int _xMax;
    private int _yMax;
    char[,] _map;
    private List<int> _data1;
    private Dictionary<(int x, int y), List<int>> _data2;

    IEnumerable<(int x, int y)> Adjacent(int x, int y)
    {
        for (var xx = x - 1; xx <= x + 1; xx++)
        for (var yy = y - 1; yy <= y + 1; yy++)
            if (xx >= 0 && xx < _xMax && yy >= 0 && yy < _yMax && !(xx == x && yy == y))
                yield return (xx, yy);
    }

    bool IsSymbol(char character) => !char.IsDigit(character) && character != '.';

    IEnumerable<(char symbol, int x, int y)> AdjacentSymbols(int x, int y) => Adjacent(x, y)
        .Select(q => (_map[q.x, q.y], q.x, q.y))
        .Where(q => IsSymbol(q.Item1));

    bool AdjacentSymbolFound(int x, int y) => AdjacentSymbols(x, y).Any();
    
    (bool found, int x, int y) AdjacentGearSymbol(int x, int y) => AdjacentSymbols(x, y)
        .Where(q => q.symbol == '*').Select(q => (true, q.x, q.y)).FirstOrDefault();

    
    protected override void Parse(List<string> data)
    {
        _xMax = data[0].Length;
        _yMax = data.Count;
        _map = new char[_xMax, _yMax];
        var y = -1;
        foreach (var line in data)
        {
            y++;
            for (int x = 0; x < _xMax; x++)
                _map[x, y] = line[x];
        }

        _data1 = new();
        _data2 = new();
        var num = new StringBuilder();
        for (y = 0; y < _xMax; y++)
        {
            var x = 0;
            while (x < _xMax)
            {
                while (x < _xMax && !char.IsDigit(_map[x, y]))
                    x++;

                num.Clear();
                var isValid = false;
                var (gearX, gearY) = (-1, -1);
                for (; x < _xMax && char.IsDigit(_map[x, y]); x++)
                {
                    num.Append(_map[x, y]);
                    isValid = isValid || AdjacentSymbolFound(x, y);
                    var gear = AdjacentGearSymbol(x, y);
                    if (gear.found)
                        (gearX, gearY) = (gear.x, gear.y);
                }

                if (num.Length > 0 && isValid)
                {
                    var numInt = num.ToString().ToInt();
                    _data1.Add(numInt);
                    if (gearX > -1)
                    {
                        if (!_data2.TryGetValue((gearX, gearY), out var gearList))
                        {
                            gearList = new List<int>();
                            _data2[(gearX, gearY)] = gearList;
                        }
                        gearList.Add(numInt);
                    }
                }
            }
        }
    }

    protected override object Solve1()
    {
        return _data1.Sum();
    }

    protected override object Solve2()
    {
        return _data2.Values.Where(q => q.Count == 2).Select(q => q[0] * q[1]).Sum();
    }
}