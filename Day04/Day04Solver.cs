namespace AdventOfCode.Day04;

public class Day04Tests
{
    private readonly ITestOutputHelper _output;
    public Day04Tests(ITestOutputHelper output) => _output = output;

    [Fact] public void Step1WithExample() => new Day04Solver().ExecuteExample1("13");
        
    [Fact] public void Step2WithExample() => new Day04Solver().ExecuteExample2("30");

    [Fact] public void Step1WithPuzzleInput() => _output.WriteLine(new Day04Solver().ExecutePuzzle1());
        
    [Fact] public void Step2WithPuzzleInput() => _output.WriteLine(new Day04Solver().ExecutePuzzle2());
}

public class Day04Solver : SolverBase
{
    class Game
    {
        public List<int> WinningNumbers { get; set; }
        public List<int> YourNumbers { get; set; }
        public int Instances { get; set; } = 1;

        public int MatchCount() => YourNumbers.Count(q => WinningNumbers.Contains(q));

        public int Points()
        {
          var count = MatchCount();
          return count == 0 ? 0 : 1 << (count - 1);
        } 
    }
    
    List<Game> _data;

    protected override void Parse(List<string> data)
    {
        _data = new();
        foreach (var line in data)
        {
            var game = new Game();
            _data.Add(game);
            var parts = line.Split(new[] { ':', '|' });
            game.WinningNumbers = parts[1]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(q => q.ToInt())
                .ToList();
            game.YourNumbers = parts[2]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(q => q.ToInt())
                .ToList();
        }
    }

    protected override object Solve1()
    {
        return _data.Sum(q => q.Points());
    }

    protected override object Solve2()
    {
        for (int i = 0; i < _data.Count; i++)
        {
            var game = _data[i];
            var matches = game.MatchCount();
            for (int j = 1; j <= matches; j++)
                _data[i + j].Instances += game.Instances;
        }

        return _data.Sum(q => q.Instances);
    }
}
