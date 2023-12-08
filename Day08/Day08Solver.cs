namespace AdventOfCode.Day08;

public class Day08Tests
{
    private readonly ITestOutputHelper _output;
    public Day08Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day08Solver().ExecuteExample1("6");

    [Fact]
    public void Step2WithExample() => new Day08Solver().ExecuteExample2("6");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day08Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day08Solver().ExecutePuzzle2());
}

public class Day08Solver : SolverBase
{
    Dictionary<string, (string Left, string Right)> _data;
    private char[] _directions;
    private List<string> _starts;

    protected override void Parse(List<string> data)
    {
        _data = new();
        _directions = data[0].ToCharArray();
        _starts = new();

        foreach (var line in data.Skip(2))
        {
            var elements = line.Split(new[] { '=', '(', ')', ',' },
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            _data.Add(elements[0], (elements[1], elements[2]));
            if (elements[0].EndsWith('A'))
                _starts.Add(elements[0]);
        }
    }

    private char Direction(int count) => _directions[count % _directions.Length];
    private char Direction(long count) => _directions[count % _directions.Length];
    private string Step(string position, int count) => Step(position, Direction(count));

    private string Step(string position, char direction) => direction
        switch
        {
            'L' => _data[position].Left,
            _ => _data[position].Right
        };

    protected override object Solve1()
    {
        var position = "AAA";
        var count = 0;
        while (position != "ZZZ")
            position = Step(position, count++);

        return count;
    }

    protected override object Solve2()
    {
        static long GCD(long a, long b)
        {
            if (a == 0)
                return b;

            while (b != 0)
            {
                if (a > b)
                    a -= b;
                else
                    b -= a;
            }
            return a;
        }

        static long LCM(long a, long b) => (a * b) / GCD(a, b);


        var positions = _starts.ToArray();
        var counts = new int[positions.Length];
        for (var i = 0; i < positions.Length; i++)
        {
            var position = positions[i];
            var count = 0;
            while (!position.EndsWith("Z"))
                position = Step(position, count++);
            counts[i] = count;
        }

        var result = (long)counts[0];
        foreach (var count in counts.Skip(1))
            result = LCM(result, count);

        return result;
    }
}