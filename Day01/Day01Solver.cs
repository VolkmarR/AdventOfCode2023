namespace AdventOfCode.Day01;

public class Day01Tests
{
    private readonly ITestOutputHelper _output;
    public Day01Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day01Solver().ExecuteExample1("142");

    [Fact]
    public void Step2WithExample() => new Day01Solver().ExecuteExample2("281");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day01Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day01Solver().ExecutePuzzle2());
}

public class Day01Solver : SolverBase
{
    List<int> _data1;
    List<int> _data2;

    protected override void Parse(List<string> data)
    {
        var digitsTest = new List<(string, char)>()
        {
            ("one", '1'),
            ("two", '2'),
            ("three", '3'),
            ("four", '4'),
            ("five", '5'),
            ("six", '6'),
            ("seven", '7'),
            ("eight", '8'),
            ("nine", '9'),
            ("1", '1'),
            ("2", '2'),
            ("3", '3'),
            ("4", '4'),
            ("5", '5'),
            ("6", '6'),
            ("7", '7'),
            ("8", '8'),
            ("9", '9'),
        };

        char FindFirstDigit(string line)
        {
            var index = int.MaxValue;
            var result = ' ';
            foreach (var (text, value) in digitsTest)
            {
                var current = line.IndexOf(text);
                if (current > -1 && current < index)
                    (index, result) = (current, value);
            }

            return result;
        }

        char FindlastDigit(string line)
        {
            var index = int.MinValue;
            var result = ' ';
            foreach (var (text, value) in digitsTest)
            {
                var current = line.LastIndexOf(text);
                if (current > -1 && current > index)
                    (index, result) = (current, value);
            }

            return result;
        }

        _data1 = new();
        _data2 = new();
        foreach (var line in data)
        {
            var first = line.FirstOrDefault(q => char.IsDigit(q));
            var last = line.LastOrDefault(q => char.IsDigit(q));
            _data1.Add(Int32.Parse(string.Concat(first, last)));

            first = FindFirstDigit(line);
            last = FindlastDigit(line);
            _data2.Add(Int32.Parse(string.Concat(first, last)));
        }
    }

    protected override object Solve1()
    {
        return _data1.Sum();
    }

    protected override object Solve2()
    {
        return _data2.Sum();
    }
}