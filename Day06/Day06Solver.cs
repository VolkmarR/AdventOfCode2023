using System.Linq;

namespace AdventOfCode.Day06;

public class Day06Tests
{
    private readonly ITestOutputHelper _output;
    public Day06Tests(ITestOutputHelper output) => _output = output;

    [Fact] public void Step1WithExample() => new Day06Solver().ExecuteExample1("288");

    [Fact] public void Step2WithExample() => new Day06Solver().ExecuteExample2("71503");

    [Fact] public void Step1WithPuzzleInput() => _output.WriteLine(new Day06Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => _output.WriteLine(new Day06Solver().ExecutePuzzle2());
}

public class Day06Solver : SolverBase
{
    List<string> _data;

    protected override void Parse(List<string> data)
    {
        _data = data;
    }

    protected override object Solve1()
    {
        var times = _data[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(q => q.ToInt()).ToList();
        var distances = _data[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(q => q.ToInt()).ToList();
        var result = 1;
        for (int i = 0; i < times.Count; i++)
        {
            var time = times[i];
            var distance = distances[i];

            var count = 0;
            for (int j = 1; j < time; j++)
            {
                if (j * (time - j) > distance)
                    count++;
            }
            result *= count;
        }

        return result;
    }

    protected override object Solve2()
    {
        var time = string.Join("", _data[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)).ToLong();
        var distance = string.Join("", _data[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)).ToLong();
        var result = 1;

        var count = 0;
        for (int j = 1; j < time; j++)
        {
            if (j * (time - j) > distance)
                count++;
        }
        result *= count;

        return result;
    }
}
