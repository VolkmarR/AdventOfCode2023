namespace AdventOfCode.Day15;

public class Day15Tests
{
    private readonly ITestOutputHelper _output;
    public Day15Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day15Solver().ExecuteExample1("1320");

    [Fact]
    public void Step2WithExample() => new Day15Solver().ExecuteExample2("145");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day15Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day15Solver().ExecutePuzzle2());
}

public class Day15Solver : SolverBase
{
    List<string> _data;
    private Dictionary<int, List<(string label, int focalLength)>> _boxes;

    protected override void Parse(List<string> data)
    {
        _data = data.First().Split(',').ToList();
        _boxes = new();
        for (int i = 0; i < 256; i++)
            _boxes[i] = new();
    }

    int GetHashValue(string data)
    {
        var result = 0;
        foreach (var character in data)
        {
            result += (int)character;
            result *= 17;
            result %= 256;
        }

        return result;
    }

    void ExecuteOperation(string element)
    {
        var parts = element.Split(new[] { '-', '=' }, StringSplitOptions.RemoveEmptyEntries);
        var label = parts[0];
        var box = GetHashValue(label);
        var slots = _boxes[box];
        if (parts.Length == 1) // -
            slots.RemoveAll(q => q.label == label);
        else
        {
            var focalLength = parts[1].ToInt();
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].label == label)
                {
                    slots[i] = (label, focalLength);
                    return;
                }
            }
            slots.Add((label, focalLength));
        }
    }

    long SumBoxes()
    {
        var sum = 0L;
        for (int box = 0; box <= 255; box++)
        {
            var slots = _boxes[box];
            for (int i = 0; i < slots.Count; i++)
                sum += (box + 1) * (i + 1) * slots[i].focalLength;
        }

        return sum;
    }

    protected override object Solve1()
    {
        return _data.Sum(GetHashValue);
    }

    protected override object Solve2()
    {
        foreach (var operation in _data)
            ExecuteOperation(operation);
        return SumBoxes();
    }
}