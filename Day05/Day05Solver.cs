namespace AdventOfCode.Day05;

public class Day05Tests
{
    private readonly ITestOutputHelper _output;
    public Day05Tests(ITestOutputHelper output) => _output = output;

    [Fact] public void Step1WithExample() => new Day05Solver().ExecuteExample1("35");

    [Fact] public void Step2WithExample() => new Day05Solver().ExecuteExample2("46");

    [Fact] public void Step1WithPuzzleInput() => _output.WriteLine(new Day05Solver().ExecutePuzzle1());

    [Fact] public void Step2WithPuzzleInput() => _output.WriteLine(new Day05Solver().ExecutePuzzle2());
}

public class Day05Solver : SolverBase
{
    List<long> _seeds;
    Dictionary<string, Map> _maps = new();

    class Range
    {
        public long FromBegin { get; set; }
        public long FromEnd { get; set; }
        public long ToBegin { get; set; }
        public long ToEnd { get; set; }
    }

    class Map
    {
        public string From { get; set; }
        public string To { get; set; }
        public List<Range> Ranges { get; set; } = new();

        public long MapFrom(long value)
        {
            foreach (var item in Ranges)
            {
                if (item.FromBegin <= value && value <= item.FromEnd)
                    return item.ToBegin + value - item.FromBegin;
            }

            return value;
        }
    }


    protected override void Parse(List<string> data)
    {
        _seeds = data[0].Split(' ').Skip(1).Select(q => q.ToLong()).ToList();
        Map map = null;
        foreach (var line in data.Skip(2).Where(q => !string.IsNullOrEmpty(q)))
        {
            if (!char.IsDigit(line[0]))
            {
                var parts = line.Split(new char[] { ' ', '-' });
                map = new Map { From = parts[0], To = parts[2] };
                _maps[map.From] = map;
            }
            else
            {
                var parts = line.Split(' ').Select(q => q.ToLong()).ToArray();
                map.Ranges.Add(new Range { ToBegin = parts[0], ToEnd = parts[0] + parts[2] - 1, FromBegin = parts[1], FromEnd = parts[1] + parts[2] - 1 });
            }
        }
    }

    private long GetLocation(long seed)
    {
        var nextMap = "seed";
        var current = seed;
        while (_maps.TryGetValue(nextMap, out var map))
        {
            nextMap = map.To;
            current = map.MapFrom(current);
        }
        return current;
    }

    protected override object Solve1()
    {
        var result = long.MaxValue;
        foreach (var seed in _seeds)
        {
            var location = GetLocation(seed);
            result = Math.Min(result, location);
        }

        return result;
    }

    protected override object Solve2()
    {
        var result = long.MaxValue;
        for (var i = 0; i < _seeds.Count; i+=2)
        {
            for (long seed = _seeds[i]; seed < _seeds[i] + _seeds[i + 1]; seed++)
            {
                var location = GetLocation(seed);
                result = Math.Min(result, location);
            }
        }
        return result;
    }
}
