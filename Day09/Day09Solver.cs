namespace AdventOfCode.Day09;

public class Day09Tests
{
    private readonly ITestOutputHelper _output;
    public Day09Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day09Solver().ExecuteExample1("114");

    [Fact]
    public void Step2WithExample() => new Day09Solver().ExecuteExample2("??");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day09Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day09Solver().ExecutePuzzle2());
}

public class Day09Solver : SolverBase
{
    List<int[]> _data;

    protected override void Parse(List<string> data)
    {
        _data = new();
        foreach (var line in data)
            _data.Add(line.Split(' ').Select(q => q.ToInt()).ToArray());
    }

    private int GetNextPrediction(int[] data)
    {
        var steps = GetPredictionSteps(data);
        var nextValue = 0;
        for (var i = steps.Count - 2; i >= 0; i--)
            nextValue = steps[i].Last() + nextValue;
        return nextValue;
    }

    private int GetPreviousPrediction(int[] data)
    {
        var steps = GetPredictionSteps(data);
        var previousValue = 0;
        for (var i = steps.Count - 2; i >= 0; i--)
            previousValue = steps[i].First() - previousValue;
        return previousValue;
    }

    
    private static List<int[]> GetPredictionSteps(int[] data)
    {
        var allZero = data.All(q => q == 0);
        var steps = new List<int[]>() { data };
        var step = data;
        while (!allZero)
        {
            allZero = true;
            var nextStep = new int[step.Length - 1];
            for (var i = 0; i < nextStep.Length; i++)
            {
                nextStep[i] = step[i + 1] - step[i];
                allZero = allZero && nextStep[i] == 0;
            }

            steps.Add(nextStep);
            step = nextStep;
        }

        return steps;
    }

    protected override object Solve1() => _data.Sum(data => GetNextPrediction(data));

    protected override object Solve2() => _data.Sum(data => GetPreviousPrediction(data));
}