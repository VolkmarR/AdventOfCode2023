namespace AdventOfCode.Day12;

public class Day12Tests
{
    private readonly ITestOutputHelper _output;
    public Day12Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day12Solver().ExecuteExample1("??");

    [Fact]
    public void Step2WithExample() => new Day12Solver().ExecuteExample2("??");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day12Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day12Solver().ExecutePuzzle2());
}

public class Day12Solver : SolverBase
{
    record Row(char[] Springs, int[] Groups);

    List<Row> _data;

    protected override void Parse(List<string> data)
    {
        _data = new();
        foreach (var line in data)
        {
            var parts = line.Split(new[] { ' ', ',' });
            _data.Add(new(parts[0].ToCharArray(), parts.Skip(1).Select(q => q.ToInt()).ToArray()));
        }
    }

    private int GetVariants(Row data, int springIndex, int springGroupIndex)
    {
        int SkipEmpty(int start)
        {
            while (start < data.Springs.Length && data.Springs[start] == '.')
                start++;
            return start;
        }

        (int end, int unknown) GetBlockEnd(int start)
        {
            var unknown = 0;
            while (start < data.Springs.Length && data.Springs[start] != '.')
            {
                if (data.Springs[start] == '?')
                    unknown++;
                start++;
            }
            return (start, unknown);
        }
        
        while (springIndex < data.Springs.Length && springGroupIndex < data.Groups.Length)
        {
            springIndex = SkipEmpty(springIndex);
            var (blockend, unknown) = GetBlockEnd(springIndex);
            var blockWidth = blockend - springIndex;
            if (unknown == 0)
            {
                springGroupIndex++;
            }
        }

        return 1;
    }


    
    private int GetVariants(Row data) => GetVariants(data, 0, 0);

    protected override object Solve1() => _data.Sum(GetVariants);

    protected override object Solve2()
    {
        throw new Exception("Solver error");
    }
}