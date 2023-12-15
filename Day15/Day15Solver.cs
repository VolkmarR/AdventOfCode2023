namespace AdventOfCode.Day15;

public class Day15Tests
{
    private readonly ITestOutputHelper _output;
    public Day15Tests(ITestOutputHelper output) => _output = output;

    [Fact] public void Step1WithExample() => new Day15Solver().ExecuteExample1("1320");
        
    [Fact] public void Step2WithExample() => new Day15Solver().ExecuteExample2("??");

    [Fact] public void Step1WithPuzzleInput() => _output.WriteLine(new Day15Solver().ExecutePuzzle1());
        
    [Fact] public void Step2WithPuzzleInput() => _output.WriteLine(new Day15Solver().ExecutePuzzle2());
}

public class Day15Solver : SolverBase
{
    List<string> _data;

    protected override void Parse(List<string> data)
    {
        _data = data.First().Split(',').ToList();
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
    
    protected override object Solve1()
    {
        return _data.Sum(GetHashValue);
    }

    protected override object Solve2()
    {
        throw new Exception("Solver error");
    }
}
